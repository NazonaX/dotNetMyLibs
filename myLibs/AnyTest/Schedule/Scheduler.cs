using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AnyTest.Schedule
{
    /// <summary>
    /// 资源调度的调度器类。
    /// </summary>
    public class Scheduler
    {
        
        private ResourceMapper _resourceMapper;
        private List<Task> _tasks = new List<Task>();

        public Scheduler AddResource(String key, Joint joint)
        {
            _resourceMapper.AddJoint(key, joint);
            return this;
        }
        public Joint GetResource(String key)
        {
            return _resourceMapper.GetJoint(key);
        }
        public Scheduler AddPreMissionToLatestTask(Joint from, Joint to, MissionType type, double timeCost)
        {
            if (_tasks.Count == 0)
                throw new Exception("Task Number ERROR...");
            _tasks[_tasks.Count - 1].AddNewPreMission(
                new Mission(from, to, _tasks[_tasks.Count - 1], type, timeCost, this._resourceMapper));
            return this;
        }
        public Scheduler AddDoMissionToLatestTask(Joint from, Joint to)
        {
            if (_tasks.Count == 0)
                throw new Exception("Task Number ERROR...");
            _tasks[_tasks.Count - 1].AddNewDoMission(
                new Mission(from, to, _tasks[_tasks.Count - 1], MissionType.DoWork, 0, this._resourceMapper));
            return this;
        }
        public Scheduler NewTask(String taskName, int layer, int rack, int column, int targetRack)
        {
            _tasks.Add(new Task(taskName, layer, rack, column, targetRack));
            return this;
        }
        public Scheduler AddTask(Task task)
        {
            _tasks.Add(task);
            return this;
        }

        public void OutPutOriginalSequence()
        {
            bool originalMode = Debugger.GetDebugMode();
            Debugger.SetDebugMode(true);
            CalculateSequence(this._tasks);
            Debugger.WriteLine("============================================");
            Debugger.SetDebugMode(originalMode);
        }

        public void OutputSequence(List<Task> tasks)
        {
            bool originalMode = Debugger.GetDebugMode();
            Debugger.SetDebugMode(true);
            Debugger.WriteLine("+++++++++++++++++++++++++++++++++++++++++++");
            CalculateSequence(tasks);
            Debugger.WriteLine("+++++++++++++++++++++++++++++++++++++++++++");
            Debugger.SetDebugMode(originalMode);
        }

        public OptimizedResult OptimizeRecursion()
        {
            //具体优化方案在此
            OptimizedResult result = RecursionMethodOptimize(this._tasks);
            return result;
        }

        public OptimizedResult Optimize()
        {
            //具体优化方案在此
            //至少计算windowSize*batchSize次
            OptimizedResult result = HeuristicMethodOptimize1(this._tasks, batchSize:100, 
                refreshSize:10, maxIterationCount:2000, errorRatio:0.05, eMatrixRatio:1, selectDiffElement:1);
            return result;
        }

        /// <summary>
        /// 递归的方式，O(M!)
        /// </summary>
        private OptimizedResult RecursionMethodOptimize(List<Task> tasks)
        {
            OptimizedResult result = new OptimizedResult();
            result.TimeCost = Double.MaxValue;
            IList<IList<IList<Task>>> taskMatrix = ParseTasks(tasks);
            int[][] selectedArray = new int[taskMatrix.Count][];
            for(int i = 0; i < taskMatrix.Count; i++)
            {
                selectedArray[i] = new int[taskMatrix[i].Count];
            }
            Task[] tasksArray = new Task[tasks.Count];
            DoRecursionByOptimize(taskMatrix, tasksArray, 0, selectedArray, result);
            return result;
        }
        /// <summary>
        /// 把一串任务分析成一个任务矩阵，一维是任务种类，二维是顺序可执行的任务实体。
        /// 需保证输入序列就是顺序可执行
        /// 新增同源任务，区别于同种任务，同源任务是同种任务的细致区分，表示同一货道的任务
        /// </summary>
        private IList<IList<IList<Task>>> ParseTasks(List<Task> tasks)
        {
            IList<IList<IList<Task>>> parsedTasks = new List<IList<IList<Task>>>();
            //Task种类->Task源->任务列表
            Dictionary<Task, Dictionary<int, IList<Task>>> taskDict = new Dictionary<Task, Dictionary<int, IList<Task>>>();
            foreach(Task task in tasks)
            {
                if (!taskDict.ContainsKey(task))
                {
                    taskDict.Add(task, new Dictionary<int, IList<Task>>());
                }
                int sourceKey = ((task.Rack - task.TargetRack) > 0 ? 1 : -1) * task.Column;
                if (!taskDict[task].ContainsKey(sourceKey))
                {
                    taskDict[task].Add(sourceKey, new List<Task>());
                }
                taskDict[task][sourceKey].Add(task);
            }
            //组装parsedTasks
            foreach (var taskList in taskDict.Values)
            {
                IList<IList<Task>> tl = new List<IList<Task>>();
                foreach(var sourceTaskList in taskList.Values)
                {
                    tl.Add(sourceTaskList);
                }
                parsedTasks.Add(tl);
            }
            return parsedTasks;
        }

        /// <summary>
        /// 递归执行者，结果存储于入参result
        /// </summary>
        private void DoRecursionByOptimize(IList<IList<IList<Task>>> tasks,
            Task[] tasksArray, int index,
            int[][] selectedArray,
            OptimizedResult result)
        {
            if(index == tasksArray.Length)
            {
                //计算
                double timeCost = CalculateSequence(tasksArray.ToList());
                if(result.TimeCost > timeCost)
                {
                    result.Tasks = tasksArray.ToList();
                    result.TimeCost = timeCost;
                    result.Counter = 0;
                }
                else if(result.TimeCost == timeCost)
                {
                    result.Counter++;
                }
                return;
            }
            for(int i = 0; i < selectedArray.Length; i++)
            {
                for(int j = 0; j < selectedArray[i].Length; j++)
                {
                    if (selectedArray[i][j] < tasks[i][j].Count)
                    {
                        tasksArray[index] = tasks[i][j][selectedArray[i][j]];
                        selectedArray[i][j]++;
                        DoRecursionByOptimize(tasks, tasksArray, index + 1, selectedArray, result);
                        selectedArray[i][j]--;
                        tasksArray[index] = null;
                    }
                }
            }
        }

        /// <summary>
        /// 启发式方式
        /// </summary>
        private OptimizedResult HeuristicMethodOptimize1(List<Task> tasks,
            int batchSize, int refreshSize,
            int maxIterationCount, double errorRatio,
            double eMatrixRatio, double selectDiffElement)
        {
            OptimizedResult result = new OptimizedResult();
            result.TimeCost = Double.MaxValue;
            IList<IList<IList<Task>>> taskMatrix = ParseTasks(tasks);
            Task[] tasksArray = new Task[tasks.Count];
            Random random = new Random();
            OptimizedResult[] batchResult = new OptimizedResult[(int)(batchSize * (1 - eMatrixRatio))];
            OptimizedResult[] eBatchResult = new OptimizedResult[(int)(batchSize * eMatrixRatio)];
            List<OptimizedResult> sortedList = new List<OptimizedResult>(batchSize);
            ProbabilityMatrix dProbablityMatrix = new ProbabilityMatrix(taskMatrix, tasks.Count);
            ProbabilityMatrix eProbablityMatrix = new ProbabilityMatrix(taskMatrix, tasks.Count);
            String output = "{0}::当前最优:{1:0.00},平权--均值:{2:0.00}, 标准差:{3:0.00}。动态--均值:{4:0.00}, 标准差:{5:0.00}。";
            Debugger debugger = new Debugger();

            int batchIndex = 0;
            int iteration = 0;
            while (maxIterationCount != -1 && iteration < maxIterationCount)
            {
                //计算一个，先搞平权的batch数量
                for (batchIndex = 0; batchIndex < eBatchResult.Length; batchIndex++)
                {
                    Tuple<int, int>[] selectArray = GenerateSequence(taskMatrix, tasksArray, random, eProbablityMatrix, selectDiffElement);
                    double timeCost = CalculateSequence(tasksArray.ToList());
                    iteration++;
                    OptimizedResult res = new OptimizedResult();
                    res.TimeCost = timeCost;
                    res.Tasks = tasksArray.ToList();
                    res.Selections = selectArray;
                    eBatchResult[batchIndex] = res;
                    if (timeCost < result.TimeCost)
                    {
                        result.Counter = 0;
                        result.Selections = res.Selections;
                        result.Tasks = res.Tasks;
                        result.TimeCost = res.TimeCost;
                    }
                    else if(timeCost == result.TimeCost)
                    {
                        result.Counter++;
                    }
                }
                for(batchIndex = 0; batchIndex < batchResult.Length; batchIndex++)
                {
                    Tuple<int, int>[] selectArray = GenerateSequence(taskMatrix, tasksArray, random, dProbablityMatrix, selectDiffElement);
                    double timeCost = CalculateSequence(tasksArray.ToList());
                    iteration++;
                    OptimizedResult res = new OptimizedResult();
                    res.TimeCost = timeCost;
                    res.Tasks = tasksArray.ToList();
                    res.Selections = selectArray;
                    batchResult[batchIndex] = res;
                    if (timeCost < result.TimeCost)
                    {
                        result.Counter = 0;
                        result.Selections = res.Selections;
                        result.Tasks = res.Tasks;
                        result.TimeCost = res.TimeCost;
                    }
                    else if (timeCost == result.TimeCost)
                    {
                        result.Counter++;
                    }
                }
                //计算并记录各项指标
                double avg = batchResult.Length == 0 ? 0 : batchResult.Average(b => b.TimeCost);
                double s2r = batchResult.Length == 0 ? 0 : Math.Sqrt(batchResult.Sum(b => Math.Pow(b.TimeCost - result.TimeCost, 2)) / batchResult.Length);
                double eAvg = eBatchResult.Length == 0 ? 0 : eBatchResult.Average(b => b.TimeCost);
                double eS2r = eBatchResult.Length == 0 ? 0 : Math.Sqrt(eBatchResult.Sum(b => Math.Pow(b.TimeCost - result.TimeCost, 2)) / eBatchResult.Length);
                debugger.StringAppend(iteration).StringAppend(" ")
                    .StringAppend(result.TimeCost).StringAppend(" ")
                    .StringAppend(eAvg).StringAppend(" ")
                    .StringAppend(eS2r).StringAppend(" ")
                    .StringAppend(avg).StringAppend(" ")
                    .StringAppend(s2r).StringAppend(" ")
                    .StringAppend("\n");
                Debugger.WriteLine(String.Format(output, iteration, result.TimeCost, eAvg, eS2r, avg, s2r));

                sortedList.Clear();
                sortedList.AddRange(eBatchResult);
                sortedList.AddRange(batchResult);
                sortedList.Sort(new OptimizedResultComparator());
                if (eMatrixRatio < 1)
                {
                    //更新概率矩阵
                    RefreshProbabilityMatrix(dProbablityMatrix, sortedList.Take(refreshSize).ToList());
                }
            }
            debugger.WriteTofile();
            return result;
        }
        /// <summary>
        /// 概率矩阵其实就是优选取计数器
        /// </summary>
        private void RefreshProbabilityMatrix(ProbabilityMatrix probablityMatrix, List<OptimizedResult> resultList)
        {
            foreach(OptimizedResult res in resultList)
            {
                for(int i = 0; i < res.Tasks.Count; i++)
                {
                    probablityMatrix[i, res.Selections[i].Item1]++;
                    probablityMatrix[i, res.Selections[i].Item1, res.Selections[i].Item2]++;
                }
            }
        }


        /// <summary>
        /// 随机生成一个任务序列
        /// </summary>
        private Tuple<int ,int>[] GenerateSequence(IList<IList<IList<Task>>> taskMatrix, Task[] tasksArray,
            Random random, ProbabilityMatrix probabilityMatrix,
            double selectDiffElement)
        {
            Tuple<int, int>[] selectionArray = new Tuple<int, int>[tasksArray.Length];
            //Tuple->kind index->source index->count
            int[][] indexSelected = new int[taskMatrix.Count][];
            for (int i = 0; i < taskMatrix.Count; i++)
            {
                indexSelected[i] = new int[taskMatrix[i].Count];
                for(int j = 0; j < taskMatrix[i].Count; j++)
                {
                    indexSelected[i][j] = taskMatrix[i][j].Count;
                }
            }
            int index = 0;
            List<int> probIndex = new List<int>();
            List<double> probList = new List<double>();
            while(index < tasksArray.Length)
            {
                double sum = 0;
                probList.Clear();
                probIndex.Clear();
                //加入择异因子
                for (int i = 0; i < taskMatrix.Count; i++)
                {
                    bool lastSameHint = false;
                    if(index > 0 && selectionArray[index - 1].Item1 == i)
                    {
                        lastSameHint = true;
                    }
                    if (indexSelected[i].Sum() > 0)
                    {
                        double proValue = lastSameHint ? (probabilityMatrix[index, i] / selectDiffElement) : probabilityMatrix[index, i];
                        sum += proValue;
                        probIndex.Add(i);
                        probList.Add(proValue);
                    }
                }
                int selectKind = RandomSelect(random, probIndex, probList, sum);
                probList.Clear();
                probIndex.Clear();
                sum = 0;
                for(int i = 0; i < taskMatrix[selectKind].Count; i++)
                {
                    if(indexSelected[selectKind][i] > 0)
                    {
                        sum += probabilityMatrix[index, selectKind, i];
                        probIndex.Add(i);
                        probList.Add(probabilityMatrix[index, selectKind, i]);
                    }
                }
                int selectSource = RandomSelect(random, probIndex, probList, sum);
                int taskIndex = selectKind;
                int sourceIndex = selectSource;
                int itemIndex = taskMatrix[selectKind][selectSource].Count - indexSelected[selectKind][selectSource];
                selectionArray[index] = new Tuple<int, int>(taskIndex, sourceIndex);
                tasksArray[index++] = taskMatrix[taskIndex][sourceIndex][itemIndex];
                indexSelected[selectKind][selectSource]--;
            }
            //设置后置复位任务关键节点
            //for(int i = 0; i < tasksArray.Length; i++)
            //{
            //    tasksArray[i].RecoverJoint = null;
            //    for(int j = i + 1; j < tasksArray.Length; j++)
            //    {
            //        if(selectionArray[i].Item1 == selectionArray[j].Item1)
            //        {
            //            tasksArray[i].RecoverJoint = tasksArray[j].GetUsingRailJoint();
            //            break;
            //        }
            //    }
            //}
            return selectionArray;
        }

        private int RandomSelect(Random random, List<int> probIndex, List<double> probList, double sum)
        {
            double hint = random.NextDouble();
            //Console.WriteLine(hint);
            double localSum = 0;
            for(int i = 0; i < probList.Count; i++)
            {
                localSum += probList[i];
                if(localSum / sum > hint)
                {
                    return probIndex[i];
                }
            }
            return -1;
        }

        /// <summary>
        /// 计算一个特定序列任务的流水线压缩时间
        /// </summary>
        private double CalculateSequence(List<Task> tasks)
        {
            this._resourceMapper.ResetResourceStatus();
            double clipTime = 0;
            double longestPostTime = 0;
            Dictionary<Task, double> postDictionary = new Dictionary<Task, double>();
            foreach(Task task in tasks)
            {
                double startTime = clipTime;
                if (postDictionary.ContainsKey(task))
                    startTime = postDictionary[task];
                double preMustCostTime = task.GetPreMustWorkTime(this._resourceMapper);
                task.ChangeResourceStatusPreMustMissions(this._resourceMapper);
                Debugger.WriteLine(task.TaskName + " must pre start at " + startTime + ", taking " + preMustCostTime
                    + ", end at " + (startTime + preMustCostTime));
                double preCostTime = task.GetPreWorkTime(this._resourceMapper);
                task.ChangeResourceStatusPreMissions(this._resourceMapper);
                clipTime = startTime + preCostTime + preMustCostTime;
                Debugger.WriteLine(task.TaskName + " all pre start at " + startTime + ", taking " + (preCostTime + preMustCostTime)
                    + ", ent at " + clipTime);
                double postTime = task.GetDoWorkTime(this._resourceMapper);
                task.ChangeResourceStatusDoWorkMissions(this._resourceMapper);
                Debugger.WriteLine(task.TaskName + " do start at " + clipTime + ", taking " + postTime
                    + ", end at " + (clipTime + postTime));
                double postMustTime = task.GetPostMustWorkTime(this._resourceMapper);
                task.ChangeResourcesStatusPostMustMission(this._resourceMapper);
                Debugger.WriteLine(task.TaskName + " do post recover mission at " + (clipTime + postTime) + ", taking " + (postMustTime)
                     + ", end at " + (clipTime + postMustTime + postTime));
                //更新clipTime和longestTime
                postTime += postMustTime;
                longestPostTime = longestPostTime < (clipTime + postTime) ? (clipTime + postTime) : longestPostTime;
                RefreshPostDictionary(postDictionary, clipTime, task, postTime);
            }
            return longestPostTime;
        }
        /// <summary>
        /// 根据传入的clipTime和postTime更新字典
        /// 字典中时间小于clipTime的会被删除
        /// </summary>
        private void RefreshPostDictionary(Dictionary<Task, double> postDictionary, double clipTime, Task task, double postTime)
        {
            if (postDictionary.ContainsKey(task))
            {
                postDictionary.Remove(task);
            }
            foreach(KeyValuePair<Task, double> kv in postDictionary.ToList())
            {
                if (kv.Value <= clipTime)
                    postDictionary.Remove(kv.Key);
            }
            postDictionary.Add(task, clipTime + postTime);
        }

        public static Scheduler DefaultTestScheduler1()
        {
            Scheduler scheduler = new Scheduler();
            scheduler._resourceMapper = ResourceMapper.DefaultResourceMapper();
            //装配各种默认测试用的资源点joint，mission和task
            Joint input1 = new Joint("Input point 0-1-1", 0, 1, 1, JointType.InPoint);
            scheduler.AddResource(input1.JointName, input1);
            Joint lifter1 = new Joint("Lifter 0-2-2", 0, 2, 2, JointType.Lifter);
            scheduler.AddResource(lifter1.JointName, lifter1);
            Joint rail1 = new Joint("Rail 0-2-3", 0, 2, 3, JointType.Rail);
            scheduler.AddResource(rail1.JointName, rail1);
            Joint lifter2 = new Joint("Lifter 1-2-2", 1, 2, 2, JointType.Lifter);
            scheduler.AddResource(lifter2.JointName, lifter2);
            Joint lifter3 = new Joint("Lifter 2-2-2", 2, 2, 2, JointType.Lifter);
            scheduler.AddResource(lifter3.JointName, lifter3);
            Joint rail2 = new Joint("Rail 1-2-3", 1, 2, 3, JointType.Rail);
            scheduler.AddResource(rail2.JointName, rail2);
            Joint rail3 = new Joint("Rail 2-2-3", 2, 2, 3, JointType.Rail);
            scheduler.AddResource(rail3.JointName, rail3);
            Joint lifter4 = new Joint("Lifter 0-14-2", 0, 14, 2, JointType.Lifter);
            scheduler.AddResource(lifter4.JointName, lifter4);
            Joint lifter5 = new Joint("Lifter 6-14-2", 6, 14, 2, JointType.Lifter);
            scheduler.AddResource(lifter5.JointName, lifter5);
            Joint rail4 = new Joint("Rail 6-14-3", 6, 14, 3, JointType.Rail);
            scheduler.AddResource(rail4.JointName, rail4);
            for(int i = 1; i <= 3; i++)
            {
                scheduler.AddTask(GenerateTask1(scheduler._resourceMapper, "Task 1" + i, 1, 2, 7, 13 - i));
            }
            for(int i = 1; i <= 3; i++)
            {
                scheduler.AddTask(GenerateTask2(scheduler._resourceMapper, "Task 2" + i, 0, 2, 10, 13 - i));
            }
            for (int i = 1; i <= 3; i++)
            {
                scheduler.AddTask(GenerateTask3(scheduler._resourceMapper, "Task 3" + i, 2, 2, 2, 13 - i));
            }
            for (int i = 1; i <= 3; i++)
            {
                scheduler.AddTask(GenerateTask4(scheduler._resourceMapper, "Task 4" + i, 6, 14, 300, 25 - i));
            }
            //for (int i = 1; i <= 10; i++)
            //{
            //    scheduler.AddTask(GenerateTask1(scheduler._resourceMapper, "Task 5" + i, 1, 2, 20, 13 - i));
            //}
            return scheduler;
        }

        /// <summary>
        /// 生成1类型任务，执行任务长度根据rack和targetRack决定入库货道深度
        /// </summary>
        private static Task GenerateTask1(ResourceMapper resourceMapper, String taskName, int layer, int rack, int column, int targetRack)
        {
            Task task = new Task(taskName, layer, rack, column, targetRack);
            task.AddNewPreMission(new Mission(resourceMapper.GetJoint("Rail 1-2-3"), resourceMapper.GetJoint("Lifter 1-2-2"), task, MissionType.Transmission, 7, resourceMapper));
            task.AddNewPreMission(new Mission(resourceMapper.GetJoint("Lifter 1-2-2"), resourceMapper.GetJoint("Lifter 0-2-2"), task, MissionType.Lift, 0, resourceMapper));
            task.AddNewPreMission(new Mission(resourceMapper.GetJoint("Lifter 0-2-2"), resourceMapper.GetJoint("Input point 0-1-1"), task, MissionType.Transmission, 6, resourceMapper));
            task.AddNewPreMission(new Mission(resourceMapper.GetJoint("Input point 0-1-1"), resourceMapper.GetJoint("Input point 0-1-1"), task, MissionType.Transmission, 2, resourceMapper));
            task.AddNewPreMission(new Mission(resourceMapper.GetJoint("Input point 0-1-1"), resourceMapper.GetJoint("Lifter 0-2-2"), task, MissionType.Transmission, 6, resourceMapper));
            task.AddNewPreMission(new Mission(resourceMapper.GetJoint("Lifter 0-2-2"), resourceMapper.GetJoint("Lifter 1-2-2"), task, MissionType.Lift, 0, resourceMapper));
            task.AddNewPreMission(new Mission(resourceMapper.GetJoint("Lifter 1-2-2"), resourceMapper.GetJoint("Rail 1-2-3"), task, MissionType.Transmission, 7, resourceMapper));
            task.AddNewDoMission(new Mission(resourceMapper.GetJoint("Rail 1-2-3"), new Joint("", layer, rack, column, JointType.Rail), task, MissionType.DoWork, 0, resourceMapper));
            task.AddNewDoMission(new Mission(new Joint("", layer, rack, column, JointType.Rail), new Joint("", layer, targetRack, column, JointType.Rail), task, MissionType.DoWork, 0, resourceMapper));
            task.AddNewDoMission(new Mission(new Joint("", layer, targetRack, column, JointType.Rail), new Joint("", layer, rack, column, JointType.Rail), task, MissionType.DoWork, 0, resourceMapper));
            return task;
        }
        private static Task GenerateTask2(ResourceMapper resourceMapper, String taskName, int layer, int rack, int column, int targetRack)
        {
            Task task = new Task(taskName, layer, rack, column, targetRack);
            task.AddNewPreMission(new Mission(resourceMapper.GetJoint("Rail 0-2-3"), resourceMapper.GetJoint("Lifter 0-2-2"), task, MissionType.Transmission, 5, resourceMapper));
            task.AddNewPreMission(new Mission(resourceMapper.GetJoint("Lifter 0-2-2"), resourceMapper.GetJoint("Input point 0-1-1"), task, MissionType.Transmission, 6, resourceMapper));
            task.AddNewPreMission(new Mission(resourceMapper.GetJoint("Input point 0-1-1"), resourceMapper.GetJoint("Input point 0-1-1"), task, MissionType.Transmission, 2, resourceMapper));
            task.AddNewPreMission(new Mission(resourceMapper.GetJoint("Input point 0-1-1"), resourceMapper.GetJoint("Lifter 0-2-2"), task, MissionType.Transmission, 6, resourceMapper));
            task.AddNewPreMission(new Mission(resourceMapper.GetJoint("Lifter 0-2-2"), resourceMapper.GetJoint("Rail 0-2-3"), task, MissionType.Transmission, 5, resourceMapper));
            task.AddNewDoMission(new Mission(resourceMapper.GetJoint("Rail 0-2-3"), new Joint("", layer, rack, column, JointType.Rail), task, MissionType.DoWork, 0, resourceMapper));
            task.AddNewDoMission(new Mission(new Joint("", layer, rack, column, JointType.Rail), new Joint("", layer, targetRack, column, JointType.Rail), task, MissionType.DoWork, 0, resourceMapper));
            task.AddNewDoMission(new Mission(new Joint("", layer, targetRack, column, JointType.Rail), new Joint("", layer, rack, column, JointType.Rail), task, MissionType.DoWork, 0, resourceMapper));
            return task;
        }
        private static Task GenerateTask3(ResourceMapper resourceMapper, String taskName, int layer, int rack, int column, int targetRack)
        {
            Task task = new Task(taskName, layer, rack, column, targetRack);
            task.AddNewPreMission(new Mission(resourceMapper.GetJoint("Rail 2-2-3"), resourceMapper.GetJoint("Lifter 2-2-2"), task, MissionType.Transmission, 5, resourceMapper));
            task.AddNewPreMission(new Mission(resourceMapper.GetJoint("Lifter 2-2-2"), resourceMapper.GetJoint("Lifter 0-2-2"), task, MissionType.Lift, 0, resourceMapper));
            task.AddNewPreMission(new Mission(resourceMapper.GetJoint("Lifter 0-2-2"), resourceMapper.GetJoint("Input point 0-1-1"), task, MissionType.Transmission, 6, resourceMapper));
            task.AddNewPreMission(new Mission(resourceMapper.GetJoint("Input point 0-1-1"), resourceMapper.GetJoint("Input point 0-1-1"), task, MissionType.Transmission, 2, resourceMapper));
            task.AddNewPreMission(new Mission(resourceMapper.GetJoint("Input point 0-1-1"), resourceMapper.GetJoint("Lifter 0-2-2"), task, MissionType.Transmission, 6, resourceMapper));
            task.AddNewPreMission(new Mission(resourceMapper.GetJoint("Lifter 0-2-2"), resourceMapper.GetJoint("Lifter 2-2-2"), task, MissionType.Lift, 0, resourceMapper));
            task.AddNewPreMission(new Mission(resourceMapper.GetJoint("Lifter 2-2-2"), resourceMapper.GetJoint("Rail 2-2-3"), task, MissionType.Transmission, 5, resourceMapper));
            task.AddNewDoMission(new Mission(resourceMapper.GetJoint("Rail 2-2-3"), new Joint("", layer, rack, column, JointType.Rail), task, MissionType.DoWork, 0, resourceMapper));
            task.AddNewDoMission(new Mission(new Joint("", layer, rack, column, JointType.Rail), new Joint("", layer, targetRack, column, JointType.Rail), task, MissionType.DoWork, 0, resourceMapper));
            task.AddNewDoMission(new Mission(new Joint("", layer, targetRack, column, JointType.Rail), new Joint("", layer, rack, column, JointType.Rail), task, MissionType.DoWork, 0, resourceMapper));
            return task;
        }
        private static Task GenerateTask4(ResourceMapper resourceMapper, String taskName, int layer, int rack, int column, int targetRack)
        {
            Task task = new Task(taskName, layer, rack, column, targetRack);
            task.AddNewPreMission(new Mission(resourceMapper.GetJoint("Rail 6-14-3"), resourceMapper.GetJoint("Lifter 6-14-2"), task, MissionType.Transmission, 5, resourceMapper));
            task.AddNewPreMission(new Mission(resourceMapper.GetJoint("Lifter 6-14-2"), resourceMapper.GetJoint("Lifter 0-14-2"), task, MissionType.Lift, 0, resourceMapper));
            task.AddNewPreMission(new Mission(resourceMapper.GetJoint("Lifter 0-14-2"), resourceMapper.GetJoint("Lifter 0-2-2"), task, MissionType.Transmission, 5, resourceMapper));
            task.AddNewPreMission(new Mission(resourceMapper.GetJoint("Lifter 0-2-2"), resourceMapper.GetJoint("Input point 0-1-1"), task, MissionType.Transmission, 6, resourceMapper));
            task.AddNewPreMission(new Mission(resourceMapper.GetJoint("Input point 0-1-1"), resourceMapper.GetJoint("Input point 0-1-1"), task, MissionType.Transmission, 2, resourceMapper));
            task.AddNewPreMission(new Mission(resourceMapper.GetJoint("Input point 0-1-1"), resourceMapper.GetJoint("Lifter 0-2-2"), task, MissionType.Transmission, 6, resourceMapper));
            task.AddNewPreMission(new Mission(resourceMapper.GetJoint("Lifter 0-2-2"), resourceMapper.GetJoint("Lifter 0-14-2"), task, MissionType.Transmission, 5, resourceMapper));
            task.AddNewPreMission(new Mission(resourceMapper.GetJoint("Lifter 0-14-2"), resourceMapper.GetJoint("Lifter 6-14-2"), task, MissionType.Lift, 0, resourceMapper));
            task.AddNewPreMission(new Mission(resourceMapper.GetJoint("Lifter 6-14-2"), resourceMapper.GetJoint("Rail 6-14-3"), task, MissionType.Transmission, 5, resourceMapper));
            task.AddNewDoMission(new Mission(resourceMapper.GetJoint("Rail 6-14-3"), new Joint("", layer, rack, column, JointType.Rail), task, MissionType.DoWork, 0, resourceMapper));
            task.AddNewDoMission(new Mission(new Joint("", layer, rack, column, JointType.Rail), new Joint("", layer, targetRack, column, JointType.Rail), task, MissionType.DoWork, 0, resourceMapper));
            task.AddNewDoMission(new Mission(new Joint("", layer, targetRack, column, JointType.Rail), new Joint("", layer, rack, column, JointType.Rail), task, MissionType.DoWork, 0, resourceMapper));
            return task;
        }

    }

    public class OptimizedResult
    {
        public double TimeCost = 0;
        public int Counter = 0;
        public List<Task> Tasks = null;
        public Tuple<int, int>[] Selections = null;

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("\n");
            foreach(Task t in Tasks)
            {
                sb.Append(t.ToString()).Append("\n");
            }
            return sb.ToString();
        }
    }

    public class OptimizedResultComparator : IComparer<OptimizedResult>
    {
        public int Compare(OptimizedResult r1, OptimizedResult r2)
        {
            if (r1.TimeCost == r2.TimeCost)
                return 0;
            return r1.TimeCost > r2.TimeCost ? 1 : -1;
        }
    }

    public class ProbabilityMatrix
    {
        private double[,] railMatrix = null;
        private double[,][] kindMatrix = null;

        /// <summary>
        /// 初始化成一个二维平权概率矩阵和一个三维平权概率矩阵
        /// </summary>
        public ProbabilityMatrix(IList<IList<IList<Task>>> taskMatrix, int sequenceLength)
        {
            railMatrix = new double[sequenceLength, taskMatrix.Count];
            kindMatrix = new double[sequenceLength, taskMatrix.Count][];
            for(int i = 0; i < sequenceLength; i++)
            {
                for (int j = 0; j < taskMatrix.Count; j++)
                {
                    railMatrix[i, j] = 1;
                    kindMatrix[i, j] = new double[taskMatrix[j].Count];
                    for(int k = 0; k < taskMatrix[j].Count; k++)
                    {
                        kindMatrix[i, j][k] = 1;
                    }
                }
            }
        }

        public double this[int i, int j]
        {
            get { return railMatrix[i, j]; }
            set { railMatrix[i, j] = value; }
        }

        public double this[int i, int j, int k]
        {
            get { return kindMatrix[i, j][k]; }
            set { kindMatrix[i, j][k] = value; }
        }

    }

}

using System;
using System.Collections.Generic;
using System.Text;

namespace AnyTest.Schedule
{
    public class Task
    {
        public String TaskName { get; private set; }
        public int Layer { get; set; }
        public int Rack { get; set; }
        public int Column { get; set; }
        public int TargetRack { get; set; }
        //前置任务的不确定附加任务，通常是移动
        private List<Mission> _preMustMissionList = new List<Mission>();
        //前置任务集合
        private List<Mission> _preMissionList = new List<Mission>();
        //后置任务集合
        private List<Mission> _doMissionList = new List<Mission>();
        //后置复位任务集合
        private List<Mission> _postMustMissionList = new List<Mission>();
        public Joint RecoverJoint = null;
        
        public Task(String taskName, int layer, int rack, int column, int targetRack)
        {
            this.TaskName = taskName;
            this.Layer = layer;
            this.Rack = rack;
            this.Column = column;
            this.TargetRack = targetRack;
        }
        private Task(Task task)
        {
            //preMustMissions引用不复制，TaskName不复制，TargetRack不复制，DoMissions不复制
            this.TaskName = "";
            this.Layer = task.Layer;
            this.Rack = task.Rack;
            this.Column = task.Column;
            this.TargetRack = -1;
            this._preMissionList.AddRange(task._preMissionList);
        }
        /// <summary>
        /// 认为在同一层并且使用同一根轨道的任务属于同种任务
        /// </summary>
        public override bool Equals(object obj)
        {
            Task objTask = obj as Task;
            if (objTask == null)
                return false;
            return this.Layer == objTask.Layer
                && this.Rack == objTask.Rack;
        }

        /// <summary>
        /// 基本int的hash就是其值本身，此处考虑层放到高16位，轨道放在低16位表示其hash值
        /// </summary>
        public override int GetHashCode()
        {
            int layerHash = this.Layer.GetHashCode();
            int rackHash = this.Rack.GetHashCode();
            return layerHash << 16 | rackHash;
        }
        public override string ToString()
        {
            return this.TaskName;
        }
        /// <summary>
        /// 复制该Task。
        /// </summary>
        public Task Copy()
        {
            return new Task(this);
        }

        /// <summary>
        /// 添加一个新子任务
        /// </summary>
        public Task AddNewPreMission(Mission mission)
        {
            _preMissionList.Add(mission);
            return this;
        }

        public Task AddNewDoMission(Mission mission)
        {
            _doMissionList.Add(mission);
            return this;
        }
    

        /// <summary>
        /// 计算本任务的动态前摇时间，并生成序列的前置归位任务。
        /// </summary>
        public double GetPreMustWorkTime(ResourceMapper resourceMapper)
        {
            double preMustTimeCost = 0;
            _preMustMissionList.Clear();
            //扫描本任务的前摇任务和后摇任务所需的资源joint，以及各个mission的动作种类，
            //结合传入的resource mapper中的各个资源joint的状态，生成需要执行的前摇复位任务，
            //并计算所有前摇任务所需的时间，一般只有电梯资源点和轨道（代表子母车）会有此类操作
            HashSet<Joint> replacingLifter = new HashSet<Joint>();
            Joint jointLifter = null;
            Joint jointRail = null;
            foreach(Mission mission in this._preMissionList)
            {
                //运输任务即为传送带，为复位电梯列表添加电梯，复位至0层
                if(mission.Type == MissionType.Transmission)
                {
                    if(mission.FromJoint.Type == JointType.Lifter)
                    {
                        replacingLifter.Add(mission.FromJoint);
                    }
                    if(mission.ToJoint.Type == JointType.Lifter)
                    {
                        replacingLifter.Add(mission.ToJoint);
                    }
                }
            }
            jointRail = this._preMissionList[0].FromJoint;
            foreach (Mission mission in this._preMissionList)
            {
                //提升任务，该joint即为关键电梯资源，复位至rail位置所在层
                if (mission.Type == MissionType.Lift)
                {
                    if (mission.FromJoint.Type == JointType.Lifter
                        && mission.ToJoint.Type == JointType.Lifter)
                    {
                        replacingLifter.Remove(mission.FromJoint);
                        jointLifter = mission.FromJoint;
                    }
                    else
                    {
                        throw new Exception("Lift mission ERROR..." + mission.ToString());
                    }
                }
            }
            //生成并行复位任务，并计算最大复位时间
            //传输电梯到底层
            foreach(Joint joint in replacingLifter)
            {
                Joint from = joint.Copy();
                from.Layer = resourceMapper.GetJointStatus(from).Layer;
                from.JointName = from.AssembleJointName();
                Joint to = from.Copy();
                to.Layer = 0;
                to.JointName = to.AssembleJointName();
                if (from.Layer == to.Layer)
                    continue;
                Mission tmpMission = new Mission(from, to, this, MissionType.Lift, 0, resourceMapper);
                preMustTimeCost = preMustTimeCost < tmpMission.TimeCost ? tmpMission.TimeCost : preMustTimeCost;
                _preMustMissionList.Add(tmpMission);
                Debugger.WriteLine(tmpMission.ToString());
            }
            //生成关键电梯复位任务，并计算时间
            //关键电梯到mission的位置
            if(jointLifter != null)
            {
                Joint from = jointLifter.Copy();
                from.Layer = resourceMapper.GetJointStatus(jointLifter).Layer;
                from.JointName = from.AssembleJointName();
                Joint to = from.Copy();
                to.Layer = this.Layer;
                to.JointName = to.AssembleJointName();
                if(from.Layer != to.Layer)
                {
                    Mission tmpMission = new Mission(from, to, this, MissionType.Lift, 0, resourceMapper);
                    preMustTimeCost = preMustTimeCost < tmpMission.TimeCost ? tmpMission.TimeCost : preMustTimeCost;
                    _preMustMissionList.Add(tmpMission);
                    Debugger.WriteLine(tmpMission.ToString());
                }
            }
            //生成子母车复位任务，并计算时间
            //子母车到接轨Rail的位置
            if (jointRail == null)
                throw new Exception("Mission ERROR...At Task " + this.TaskName);
            else
            {
                Joint to = jointRail;
                Joint from = to.Copy();
                from.Column = resourceMapper.GetJointStatus(from).Column;
                from.JointName = from.AssembleJointName();
                if(from.Column != to.Column)
                {
                    Mission tempMission = new Mission(from, to, this, MissionType.DoWork, 0, resourceMapper);
                    preMustTimeCost = preMustTimeCost < tempMission.TimeCost ? tempMission.TimeCost : preMustTimeCost;
                    _preMustMissionList.Add(tempMission);
                    Debugger.WriteLine(tempMission.ToString());
                }
            }
            return preMustTimeCost;
        }
        public void ChangeResourceStatusPreMustMissions(ResourceMapper resourceMapper)
        {
            foreach(Mission mission in _preMustMissionList)
            {
                resourceMapper.GetJointStatus(mission.ToJoint).Layer = mission.ToJoint.Layer;
                resourceMapper.GetJointStatus(mission.ToJoint).Rack = mission.ToJoint.Rack;
                resourceMapper.GetJointStatus(mission.ToJoint).Column = mission.ToJoint.Column;
            }
        }
        /// <summary>
        /// 计算所有前置任务的时间和
        /// </summary>
        public double GetPreWorkTime(ResourceMapper resourceMapper)
        {
            double timeCost = 0;
            foreach(Mission mission in this._preMissionList)
            {
                if(mission.Type == MissionType.Transmission || mission.Type == MissionType.Lift)
                {
                    timeCost += mission.TimeCost;
                    Debugger.WriteLine(mission.ToString());
                }
                else
                {
                    throw new Exception("Misison not in correct list...At Task "+ this.TaskName);
                }
            }
            return timeCost;
        }
        public void ChangeResourceStatusPreMissions(ResourceMapper resourceMapper)
        {
            //此处会变的只有电梯的层
            foreach(Mission mission in _preMissionList)
            {
                if(mission.Type == MissionType.Lift)
                {
                    resourceMapper.GetJointStatus(mission.ToJoint).Layer = mission.ToJoint.Layer;
                }
            }
        }
        public double GetDoWorkTime(ResourceMapper resourceMapper)
        {
            double timeCost = 0;
            foreach (Mission mission in this._doMissionList)
            {
                if (mission.Type == MissionType.DoWork)
                {
                    timeCost += mission.TimeCost;
                    Debugger.WriteLine(mission.ToString());
                }
                else
                {
                    throw new Exception("Misison not in correct list...At Task " + this.TaskName);
                }
            }
            return timeCost;
        }
        public void ChangeResourceStatusDoWorkMissions(ResourceMapper resourceMapper)
        {
            if (_doMissionList.Count == 0)
                return;
            int index = _doMissionList.Count - 1;
            //一般最后一个指定的复位任务为最终子母车所在位置
            resourceMapper.GetJointStatus(_doMissionList[index].ToJoint).Column = _doMissionList[index].ToJoint.Column;
        }
        public double GetPostMustWorkTime(ResourceMapper resourceMapper)
        {
            this._postMustMissionList.Clear();
            if (this.RecoverJoint == null)
                return 0;
            else
            {
                Joint from = RecoverJoint.Copy();
                from.Column = resourceMapper.GetJointStatus(this.RecoverJoint).Column;
                this._postMustMissionList.Add(new Mission(from, this.RecoverJoint, this, MissionType.DoWork, 0, resourceMapper));
                return this._postMustMissionList[0].TimeCost;
            }
        }
        public void ChangeResourcesStatusPostMustMission(ResourceMapper resourceMapper)
        {
            if (_postMustMissionList.Count == 0)
                return;
            resourceMapper.GetJointStatus(_postMustMissionList[0].ToJoint).Column = _postMustMissionList[0].ToJoint.Column;
        }

        internal Joint GetUsingRailJoint()
        {
            return _preMissionList[0].FromJoint;
        }
    }
}

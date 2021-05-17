using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MapAndSimulation.Mission;

namespace MapAndSimulation.Map
{
    [Serializable]
    public class Map
    {
        private static string mapFile = "map.nax";
        private List<Layer> layers;
        private int[] directionOfMainPath;//the direction of MainPath
        //↑0-from left to right
        //↑1-from right to left
        //↑2-all directions are available
        //↑size = MainPath
        private int[] pathCountOfMainPath;//the count of path of each MainPath
        //↑size = MainPath
        private double lengthGapOfRow = 1;
        private double lengthGapOfColumn = 1;
        private double lengthHeightOfLayer = 1;

        private int[] size;
        private int[] mainPaths;
        private int[] elevators;


        public List<Layer> Layers { get => layers; set => layers = value; }
        public int[] DirectionOfMainPath { get => directionOfMainPath; set => directionOfMainPath = value; }
        public int[] PathCountOfMainPath { get => pathCountOfMainPath; set => pathCountOfMainPath = value; }
        public double LengthGapOfRow { get => lengthGapOfRow; set => lengthGapOfRow = value; }
        public double LengthGapOfColumn { get => lengthGapOfColumn; set => lengthGapOfColumn = value; }
        public double LengthHeightOfLayer { get => lengthHeightOfLayer; set => lengthHeightOfLayer = value; }
        public int[] Size { get => size; }
        public int[] MainPaths { get => mainPaths; }
        public int[] Elevators { get => elevators; }

        public static void SetMapFilePath(string filePath)
        {
            mapFile = filePath;
        }

        /// <summary>
        /// used to create a map
        /// the map should inlclude layers, storage rack
        /// 0-no storage
        /// 1-has storage
        /// -1-is main path
        /// -2-is elevator
        /// </summary>
        /// <param name="NumOfLayers">the numsber of layers</param>
        /// <param name="Size">the size of each layers,0-rows,1-columns</param>
        /// <param name="MainPath">define the main path of each layers, let each layer is the same situation</param>
        /// <param name="Elevator">define the position of elevator</param>
        public Map(int NumOfLayers, int[] Size, int[] MainPath, int[] Elevator)
        {
            layers = new List<Layer>();
            for (int i = 0; i < NumOfLayers; i++)
                layers.Add(new Layer(i + 1, Size, MainPath, Elevator));
            directionOfMainPath = new int[MainPath.Length];
            pathCountOfMainPath = new int[MainPath.Length];
            for(int i = 0; i < MainPath.Length; i++)
            {
                directionOfMainPath[i] = 2;//default is double direction
                pathCountOfMainPath[i] = 1;//default is one path
            }
            //↓default length of storage is 1
            lengthGapOfRow = 1;
            lengthGapOfColumn = 1;
            lengthHeightOfLayer = 1;

            //↓set elementary attributes
            this.size = new int[3];
            this.size[0] = NumOfLayers;
            this.size[1] = Size[0];
            this.size[2] = Size[1];
            mainPaths = new int[MainPath.Length];
            MainPath.CopyTo(mainPaths, 0);
            elevators = new int[Elevator.Length];
            Elevator.CopyTo(elevators, 0);
        }

        public Map()
        {
            //do nothing
        }

        public Map Copy()
        {
            return Utils.IOOps.CopyMemory(this) as Map;
        }

        public static bool CheckForMapFile()
        {
            return Utils.IOOps.IsFileExisted(mapFile);
        }
        public static object ReadExistedMap()
        {
            return ReadExistedMap(mapFile);
        }
        public static object ReadExistedMap(string fileName)
        {
            Utils.Logger.WriteMsgAndLog("Reading..." + fileName);
            return Utils.IOOps.ClassRead(fileName);
        }

        public static void DeleteMap(string v)
        {
            Utils.IOOps.DeleteFile(v);
        }

        public void WriteMapToFile()
        {
            WriteMapToFile(mapFile);
        }

        public void WriteMapToFile(string fileName)
        {
            Utils.Logger.WriteMsgAndLog("Writting..." + fileName);
            Utils.IOOps.ClassWrite(fileName, this);
        }

        public bool CheckForAvalibleForNow(int[] nearestPath, Position targetPosition)
        {
            return CheckForAvalibleForNowByOneMainPath(nearestPath[0], targetPosition)
                || CheckForAvalibleForNowByOneMainPath(nearestPath[1], targetPosition);
        }


        public bool CheckForAvalibleForNowByOneMainPath(int mainPath, Position targetPosition)
        {
            if (mainPath == -1)
                return false;
            int row = targetPosition.Row;
            int direction = mainPath - row < 0 ? -1 : 1;
            int addon = direction;
            while(addon != mainPath)
            {
                if (row + addon <= 0 ||
                    row + addon > size[1] ||
                    GetStatusAt(targetPosition.Layer, 
                        targetPosition.Row + addon, 
                        targetPosition.Column) == 1)
                    return false;
                if (addon + row == mainPath)
                    break;
                addon += direction;
            }
            return true;
        }

        /// <summary>
        /// Get the nearest main path by the input MainPath;
        /// the max number of could reach path is 2 and must have 1
        /// </summary>
        /// <param name="MainPath"></param>
        /// <returns>x[0]-path could reach;x[1]-path could reach;x[2]-the nearest path;
        /// -1 means no path</returns>
        public int[] NearestMainPath(Position targetPosition)
        {
            int[] path = new int[3] { -1, -1, -1 };
            int counter = 1;
            bool upperHas = true;
            bool belowHas = true;
            while (upperHas || belowHas)
            {
                if (upperHas && targetPosition.Row - counter > 0)
                {
                    if (mainPaths.Contains(targetPosition.Row - counter))
                    {
                        upperHas = false;
                        path[0] = targetPosition.Row - counter;
                    }
                }
                if (targetPosition.Row - counter == 0)
                {
                    upperHas = false;
                }
                if (belowHas && targetPosition.Row + counter <= size[1])
                {
                    if (mainPaths.Contains(targetPosition.Row + counter))
                    {
                        belowHas = false;
                        path[1] = targetPosition.Row + counter;
                    }
                }
                if (targetPosition.Row + counter > size[1])
                {
                    belowHas = false;
                }
                counter++;
            }
            if (path[0] == -1)
                path[2] = path[1];
            else if (path[1] == -1)
                path[2] = path[0];
            else
                path[2] = path[0] < path[1] ? path[0] : path[1];
            return path;
        }

        public Mission.Mission GetStuckAdditionMission(Mission.Mission m)
        {
            throw new NotImplementedException();
        }

        public int GetStcukGoodsCount(Position targetPosition, int direction)
        {
            int row = targetPosition.Row + direction;
            int counter = 0;
            while (!mainPaths.Contains(row))
            {
                if (layers.Find(l => l.LayerNum == targetPosition.Layer)
                    .Values.Find(r => r.RowNum == row)
                    .Values[targetPosition.Column - 1] == 1)
                    counter++;
                row += direction;
            }
            return counter;
        }

        /// <summary>
        /// to get the number of goods of both sides of the nearestPath;
        /// res[0]-goods at the nearestPath[0]'s side
        /// res[1]-goods at the nearestPath[1]'s side
        /// </summary>
        /// <param name="nearestPath"></param>
        /// <param name="targetPosition"></param>
        /// <returns>the size of returned value is 2</returns>
        public int[] GetGoodCountAtBothSides(int[] nearestPath, Position targetPosition)
        {
            int[] res = new int[2];
            if (nearestPath[0] == -1)
                res[0] = -1;
            else
            {
                int direction1 = nearestPath[0] - targetPosition.Row > 0 ? 1 : -1;
                int i1 = 0;
                for(i1 = 0; i1 < mainPaths.Length; i1++)
                {
                    if (mainPaths[i1] == nearestPath[0])
                        break;
                }
                res[0] = GetGoodsNumByRow(targetPosition.Layer, i1, i1 + direction1);
            }
            if (nearestPath[1] == -1)
                res[1] = -1;
            else
            {
                int direction2 = nearestPath[1] - targetPosition.Row > 0 ? 1 : -1;
                int i2 = 0;
                for (i2 = 0; i2 < mainPaths.Length; i2++)
                {
                    if (mainPaths[i2] == nearestPath[1])
                        break;
                }
                res[1] = GetGoodsNumByRow(targetPosition.Layer, i2, i2 + direction2);
            }
            return res;
        }

        private int GetGoodsNumByRow(int layer, int rowStart, int rowEnd)
        {
            if(rowStart > rowEnd)
            {
                int tmp = rowStart;
                rowStart = rowEnd;
                rowEnd = tmp;
            }
            if (rowStart == -1)
                return GetGoodsNum(layer, 1, mainPaths[rowEnd] - 1);
            else if (rowEnd == mainPaths.Length)
                return GetGoodsNum(layer, mainPaths[rowStart] + 1, size[1]);
            else
                return GetGoodsNum(layer, mainPaths[rowStart] + 1, mainPaths[rowEnd] - 1);
        }

        /// <summary>
        /// to get the blank numbers
        /// </summary>
        /// <param name="layer"></param>
        /// <param name="s"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        private int GetGoodsNum(int layer, int s, int e)
        {
            int counter = 0;
            Layer l = layers[layer - 1];
            for(int i = s - 1; i <= e - 1; i++)
            {
                for(int j = 0; j < size[2]; j++)
                {
                    if (l.Values[i].Values[j] == 0)
                        counter++;
                }
            }
            return counter;
        }

        /// <summary>
        /// To get the good of the certain location
        /// </summary>
        /// <param name="Layer"></param>
        /// <param name="Row"></param>
        /// <param name="Column"></param>
        /// <returns></returns>
        public Good GetGoodAt(int Layer, int Row, int Column)
        {
            return this.Layers.Find(l => l.LayerNum == Layer)
                .Values.Find(r => r.RowNum == Row).Goods[Column - 1];
        }
        /// <summary>
        /// To set a good into the certain location
        /// </summary>
        /// <param name="Layer"></param>
        /// <param name="Row"></param>
        /// <param name="Column"></param>
        public void SetGoodAt(Good good, int Layer, int Row, int Column)
        {
            this.Layers.Find(l => l.LayerNum == Layer)
                .Values.Find(r => r.RowNum == Row).Goods[Column - 1] = good;
        }

        public void SetGoodAt(Good good, Position targetPosition)
        {
            SetGoodAt(good, targetPosition.Layer, targetPosition.Row, targetPosition.Column);
        }

        public Good GetGoodAt(Position position)
        {
            return GetGoodAt(position.Layer, position.Row, position.Column);
        }

        public void SetStatusAt(int v, Position targetPosition)
        {
            SetStatusAt(v, targetPosition.Layer, targetPosition.Row, targetPosition.Column);
        }
        public int GetStatusAt(Position position)
        {
            return GetStatusAt(position.Layer, position.Row, position.Column);
        }
        public int GetStatusAt(int Layer, int Row, int Column)
        {
            return this.Layers.Find(l => l.LayerNum == Layer)
                .Values.Find(r => r.RowNum == Row).Values[Column - 1];
        }

        public void SetStatusAt(int status, int Layer, int Row, int Column)
        {
            int tmp = this.Layers.Find(l => l.LayerNum == Layer)
                .Values.Find(r => r.RowNum == Row).Values[Column - 1];
            if (tmp != -1 && tmp != -2 && status >= 0)
                this.Layers.Find(l => l.LayerNum == Layer)
                    .Values.Find(r => r.RowNum == Row).Values[Column - 1] = status;
        }

    }
}

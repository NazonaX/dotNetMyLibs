using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.Entity;

namespace Models.Service
{
    public interface IMapSingletonService
    {
        /// <summary>
        /// 创建一个新地图
        /// </summary>
        /// <param name="MapName">地图名称</param>
        /// <param name="layers">地图层数</param>
        /// <param name="racks">地图行数</param>
        /// <param name="columns">地图列数</param>
        /// <returns></returns>
        Entity.Map CreateNewMap(string MapName, int layers, int racks, int columns);
        /// <summary>
        /// 保存地图，为更新所有地图元素
        /// </summary>
        void SaveMap();
        /// <summary>
        /// 删除当前地图
        /// </summary>
        void DeleteMap();
        /// <summary>
        /// 获取地图
        /// </summary>
        /// <returns></returns>
        Entity.Map GetMap();
        /// <summary>
        /// 重新加载地图
        /// </summary>
        /// <returns></returns>
        Entity.Map RefreshMap();

        //for get services
        Repository.IMapItemsService GetMapItemsService();
        Repository.ITypesService GetTypesService();
        Repository.IZonesService GetZonesService();
        Repository.IGoodsService GetGoodsService();
        Repository.ISpecialConnectionService GetSpecialConnectionService();
        Repository.ICargoWaysService GetCargoWaysService();
        Repository.ICargoWaysLockService GetCargoWaysLockService();
        Repository.IRailsService GetRailsService();
        IMapLogicsService GetMapLogicsService();
        //IMapAlgorithmService GetMapAlgorithmService();

        //Some other Map Methods
        /// <summary>
        /// 通过一些逻辑，获取一个MapItem的颜色
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        string GetColor(Entity.MapItems item);
        /// <summary>
        /// 通过计算，判断一个MapItem是否是货位
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        bool IsStorage(Entity.MapItems item);
        /// <summary>
        /// 通过计算，判断一个MapItem是否含有货物
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        bool HasGood(MapItems item);
        /// <summary>
        /// 通过计算，获取一个MapItem的状态
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        MapItems.MapItemStatus GetMapItemStatus(MapItems item);

        //Get type's id which is 
        //获取地图的各个Type的Id
        int Type_GetInputId();
        int Type_GetOutputId();
        int Type_GetRailId();
        int Type_GetStorageId();
        int Type_GetLifterId();
        int Type_GetNoneId();
        int Type_GetUnavailableId();
        int Type_GetStorageRailId();
    }
}

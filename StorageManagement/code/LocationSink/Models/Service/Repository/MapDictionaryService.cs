using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using Models.Entity;

namespace Models.Service.Repository
{
    internal class MapDictionaryService: IMapDictionaryService
    {

        public Entity.Map GetMap()
        {
            DAL.MapDictionaryDA.IMapDictionary baseMapInfoDA = new DAL.MapDictionaryDA.MapDictionaryDAO();
            List<DAL.MapDictionary> dict = baseMapInfoDA.GetMapDictionary();
            if (dict.Count == 0)
                return null;
            Entity.Map map = new Entity.Map();
            //load properties
            Dictionary<string, string> tmpDict = new Dictionary<string, string>();
            foreach(DAL.MapDictionary md in dict)
            {
                tmpDict.Add(md.MapKey.Trim(), md.MapValue.Trim());
            }
            map.MapName = tmpDict[Entity.Map.KEY_MAP_NAME];
            map.LayerCount = int.Parse(tmpDict[Entity.Map.KEY_LAYER_COUNT]);
            map.RackCount = int.Parse(tmpDict[Entity.Map.KEY_RACK_COUNT]);
            map.ColumnCount = int.Parse(tmpDict[Entity.Map.KEY_COLUMN_COUNT]);
            map.GapAlongRack = double.Parse(tmpDict[Entity.Map.KEY_GAP_ALOMG_RACK]);
            map.GapAlongCloumn = double.Parse(tmpDict[Entity.Map.KEY_GAP_ALONG_COLUMN]);
            map.GapBetweenLayers = double.Parse(tmpDict[Entity.Map.KEY_GAP_BETWEEN_LAYERS]);
            map.PSMaxSpeed = double.Parse(tmpDict[Entity.Map.KEY_PS_MAXSPEED]);
            map.PSDeceleration = double.Parse(tmpDict[Entity.Map.KEY_PS_DECELERATION]);
            map.PSAcceleration = double.Parse(tmpDict[Entity.Map.KEY_PS_ACCELERATION]);
            map.CSMaxSpeed = double.Parse(tmpDict[Entity.Map.KEY_CS_MAXSPEED]);
            map.CSDeceleration = double.Parse(tmpDict[Entity.Map.KEY_CS_DECELERATION]);
            map.CSAcceleration = double.Parse(tmpDict[Entity.Map.KEY_CS_ACCELERATION]);
            map.LMaxSpeed = double.Parse(tmpDict[Entity.Map.KEY_L_MAXSPEED]);
            map.LDeceleration = double.Parse(tmpDict[Entity.Map.KEY_L_DECELERATION]);
            map.LAcceleration = double.Parse(tmpDict[Entity.Map.KEY_L_ACCELERATION]);

            return map;
        }
        public void DeleteMap(Entity.Map Map)
        {
            //Delete All infos in MapDictionary table
            //because of entity framework's delete needs Id necessarily
            //we must do the query first
            DAL.MapDictionaryDA.IMapDictionary baseMapInfoDA = new DAL.MapDictionaryDA.MapDictionaryDAO();
            List<DAL.MapDictionary> del = baseMapInfoDA.GetMapDictionary();
            baseMapInfoDA.DeleteMapDictionary(del);
        }
        public void InsertMap(Entity.Map Map)
        {
            List<DAL.MapDictionary> insertsDictionary = new List<MapDictionary>();
            insertsDictionary.Add(CreateNewMapDictionary(Entity.Map.KEY_MAP_NAME, Map.MapName));
            insertsDictionary.Add(CreateNewMapDictionary(Entity.Map.KEY_LAYER_COUNT, Map.LayerCount + ""));
            insertsDictionary.Add(CreateNewMapDictionary(Entity.Map.KEY_RACK_COUNT, Map.RackCount + ""));
            insertsDictionary.Add(CreateNewMapDictionary(Entity.Map.KEY_COLUMN_COUNT, Map.ColumnCount + ""));
            insertsDictionary.Add(CreateNewMapDictionary(Entity.Map.KEY_GAP_BETWEEN_LAYERS, Map.GapBetweenLayers + ""));
            insertsDictionary.Add(CreateNewMapDictionary(Entity.Map.KEY_GAP_ALONG_COLUMN, Map.GapAlongCloumn + ""));
            insertsDictionary.Add(CreateNewMapDictionary(Entity.Map.KEY_GAP_ALOMG_RACK, Map.GapAlongRack + ""));
            insertsDictionary.Add(CreateNewMapDictionary(Entity.Map.KEY_PS_MAXSPEED, Map.PSMaxSpeed + ""));
            insertsDictionary.Add(CreateNewMapDictionary(Entity.Map.KEY_PS_ACCELERATION, Map.PSAcceleration + ""));
            insertsDictionary.Add(CreateNewMapDictionary(Entity.Map.KEY_PS_DECELERATION, Map.PSDeceleration + ""));
            insertsDictionary.Add(CreateNewMapDictionary(Entity.Map.KEY_CS_MAXSPEED, Map.CSMaxSpeed + ""));
            insertsDictionary.Add(CreateNewMapDictionary(Entity.Map.KEY_CS_ACCELERATION, Map.CSAcceleration + ""));
            insertsDictionary.Add(CreateNewMapDictionary(Entity.Map.KEY_CS_DECELERATION, Map.CSDeceleration + ""));
            insertsDictionary.Add(CreateNewMapDictionary(Entity.Map.KEY_L_MAXSPEED, Map.LMaxSpeed + ""));
            insertsDictionary.Add(CreateNewMapDictionary(Entity.Map.KEY_L_ACCELERATION, Map.LAcceleration + ""));
            insertsDictionary.Add(CreateNewMapDictionary(Entity.Map.KEY_L_DECELERATION, Map.LDeceleration + ""));
            DAL.MapDictionaryDA.IMapDictionary baseMapInfoDA = new DAL.MapDictionaryDA.MapDictionaryDAO();
            baseMapInfoDA.InsertMapDictionary(insertsDictionary);
        }
        public void UpdateMap(Entity.Map Map)
        {
            //to update, we need to load MapDictionary first
            DAL.MapDictionaryDA.IMapDictionary baseMapInfoDA = new DAL.MapDictionaryDA.MapDictionaryDAO();
            List<DAL.MapDictionary> tmpDictionaryList = baseMapInfoDA.GetMapDictionary();
            Dictionary<string, MapDictionary> dict = new Dictionary<string, MapDictionary>();
            foreach(MapDictionary d in tmpDictionaryList)
            {
                dict.Add(d.MapKey.Trim(), d);
            }
            dict[Entity.Map.KEY_MAP_NAME].MapValue = Map.MapName;
            dict[Entity.Map.KEY_LAYER_COUNT].MapValue = Map.LayerCount + "";
            dict[Entity.Map.KEY_RACK_COUNT].MapValue = Map.RackCount + "";
            dict[Entity.Map.KEY_COLUMN_COUNT].MapValue = Map.ColumnCount + "";
            dict[Entity.Map.KEY_GAP_ALOMG_RACK].MapValue = Map.GapAlongRack + "";
            dict[Entity.Map.KEY_GAP_ALONG_COLUMN].MapValue = Map.GapAlongCloumn + "";
            dict[Entity.Map.KEY_GAP_BETWEEN_LAYERS].MapValue = Map.GapBetweenLayers + "";
            dict[Entity.Map.KEY_PS_MAXSPEED].MapValue = Map.PSMaxSpeed + "";
            dict[Entity.Map.KEY_PS_ACCELERATION].MapValue = Map.PSAcceleration + "";
            dict[Entity.Map.KEY_PS_DECELERATION].MapValue = Map.PSDeceleration + "";
            dict[Entity.Map.KEY_CS_MAXSPEED].MapValue = Map.CSMaxSpeed + "";
            dict[Entity.Map.KEY_CS_ACCELERATION].MapValue = Map.CSAcceleration + "";
            dict[Entity.Map.KEY_CS_DECELERATION].MapValue = Map.CSDeceleration + "";
            dict[Entity.Map.KEY_L_MAXSPEED].MapValue = Map.LMaxSpeed + "";
            dict[Entity.Map.KEY_L_ACCELERATION].MapValue = Map.LAcceleration + "";
            dict[Entity.Map.KEY_L_DECELERATION].MapValue = Map.LDeceleration + "";
            baseMapInfoDA.UpdateMapDictionary(tmpDictionaryList);
        }
        private MapDictionary CreateNewMapDictionary(string kEY_MAP_NAME, string mapName)
        {
            return new DAL.MapDictionary()
            {
                MapKey = kEY_MAP_NAME,
                MapValue = mapName
            };
        }

    }
}

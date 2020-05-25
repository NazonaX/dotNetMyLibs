using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.Classes;

namespace Models.Logics
{
    public class StorageManager
    {
        private static StorageManager instance = null;
        private static object locker = new object();
        private StorageManager() { }

        //double check! just for fun
        public static StorageManager Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (locker)
                    {
                        if (instance == null)
                            instance = new StorageManager();
                    }

                }
                return instance;
            }
        }

        /// <summary>
        /// to slice all good to store into some other requests based on the storage strategy
        /// </summary>
        /// <param name="goodsToStore"></param>
        /// <returns></returns>
        public List<Request> SetStoreRequests(Goods goodsToStore)
        {

            throw new NotImplementedException("StorageManager-->SetStoreRequests(Goods goodsToStore)需要具体实现。");
            //TODO:NazonaX ->The certain strategy to be implement
        }
        /// <summary>
        /// to slice all good to take out into some other requests based on the storage strategy
        /// </summary>
        /// <param name="goodsToStore"></param>
        /// <returns></returns>
        public List<Request> SetTakeOutRequests(Goods goodsToTakeOut)
        {
            throw new NotImplementedException("SorageManager-->SetTakeOutRequests(Goods goodsToTakeOut)需要具体实现。");
        }
    }
}

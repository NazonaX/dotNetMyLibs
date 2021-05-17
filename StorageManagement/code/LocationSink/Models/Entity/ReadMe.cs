using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Entity
{
    class Readme
    {
        //Entity is used for boxing the EDMX entities
        //getters and setters for connect the outer operations and boxed EDMX entities' properties
        //the Entity will be depended on with ViewModels and Repository Services
        //Models层中的Entity都是对数据库实体的进一步封装，用于对外表现和引用
        //Entity实体中含有一个对应DAL实体的引用，也即实际两份实体的引用：Entity和DAL，拥有的是一个DAL实体。
    }
}

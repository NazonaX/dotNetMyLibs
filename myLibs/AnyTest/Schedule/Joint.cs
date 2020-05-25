using System;
using System.Collections.Generic;
using System.Text;

namespace AnyTest.Schedule
{
    public class Joint
    {
        private static Debugger _debugger = new Debugger();

        public String JointName { get; set; }
        public JointType Type { get; set; }
        public int Layer { get; set; }
        public int Rack { get; set; }
        public int Column { get; set; }


        public bool IsLocked { get; set; }
        private LockJoint Locker { get; set; }
        private UnLockJoint UnLocker { get; set; }

        public Joint(String name, int layer, int rack, int column, JointType type)
        {
            this.JointName = name;
            this.Layer = layer;
            this.Rack = rack;
            this.Column = column;
            this.Type = type;
        }
        private Joint(Joint joint)
        {
            this.Layer = joint.Layer;
            this.Rack = joint.Rack;
            this.Column = joint.Column;
            this.Type = joint.Type;
        }
        public Joint Copy()
        {
            Joint joint = new Joint(this);
            joint.JointName = joint.AssembleJointName();
            return joint;
        }
        public String AssembleJointName()
        {
            return this.Type == JointType.InPoint ? "InPoint" :
                this.Type == JointType.Lifter ? "Lifter" : "Rail"
                + "-" + this.Layer + "-" + this.Rack + "-" + this.Column;
        }

        /// <summary>
        /// 为传入的joint添加依赖，既joint锁定的同时，本实体也将被锁定；解锁亦然。
        /// </summary>
        public Joint DependOn(Joint joint)
        {
            joint.Locker += this.LockSelf;
            joint.UnLocker += this.UnLockSelf;
            return this;
        }
        public void Lock()
        {
            this.LockSelf();
            Locker();
        }
        public void UnLock()
        {
            this.UnLockSelf();
            UnLocker();
        }

        private void LockSelf()
        {
            this.IsLocked = true;
            //Debugger.WriteLine(this.JointName + " Locked...");
            
        }
        private void UnLockSelf()
        {
            this.IsLocked = false;
            //Debugger.WriteLine(this.JointName + "Unlocked...");
        }

        public override string ToString()
        {
            return _debugger.StringAppend(this.JointName)
                .StringAppend(":(").StringAppend(this.Layer)
                .StringAppend("-").StringAppend(this.Rack)
                .StringAppend("-").StringAppend(this.Column)
                .StringAppend(")").BuildString();
        }
        public override int GetHashCode()
        {
            return this.Type == JointType.Lifter ?
                (this.Rack + 1) << 11 | (this.Column + 1)
                :
                this.Type == JointType.Rail ?
                    (this.Layer + 1) << 22 | (this.Rack + 1) << 11
                    :
                    (this.Layer + 1) << 22 | (this.Rack + 1) << 11 | (this.Column + 1);
        }
        public override bool Equals(object obj)
        {
            Joint joint = obj as Joint;
            if (joint == null)
                return false;
            if (joint.Type != this.Type)
                return false;
            if (this.Type == JointType.Lifter)
                return this.Rack == joint.Rack && this.Column == joint.Column;
            if (this.Type == JointType.Rail)
                return this.Layer == joint.Layer && this.Rack == joint.Rack;
            if(this.Type == JointType.InPoint)
                return this.Layer == joint.Layer && this.Rack == joint.Rack && this.Column == joint.Column;
            return false;
        }
        
    }

    public enum JointType
    {
        InPoint,
        Lifter,
        Rail
    }

    public delegate void LockJoint();
    public delegate void UnLockJoint();

}

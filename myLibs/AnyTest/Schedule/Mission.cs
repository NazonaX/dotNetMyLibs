using System;
using System.Collections.Generic;
using System.Text;

namespace AnyTest.Schedule
{
    public class Mission
    {
        public static Debugger _debugger = new Debugger();

        public Task MainTask { get; private set; }
        public Joint FromJoint { get; private set; }
        public Joint ToJoint { get; private set; }
        public double TimeCost { get; private set; }
        public MissionType Type { get; set; }

        public Mission(Joint from, Joint to, Task mainTask, MissionType type, double timeCost,
            ResourceMapper resourceMapper)
        {
            this.FromJoint = from;
            this.ToJoint = to;
            this.MainTask = mainTask;
            this.TimeCost = timeCost;
            this.Type = type;
            if(type == MissionType.Lift)
            {
                TimeCost = Calculator.CalculateTime(resourceMapper.LayerThreshold,
                    Math.Abs(this.FromJoint.Layer - this.ToJoint.Layer) * resourceMapper.LayerGap,
                    resourceMapper.LMaxSpeed,
                    resourceMapper.LAcceleration,
                    resourceMapper.LDeceleration);
            }
            else if(type == MissionType.DoWork)
            {
                //column不同的DoWork类型，表示母车运行
                if(this.FromJoint.Layer == this.ToJoint.Layer
                    && this.FromJoint.Rack == this.ToJoint.Rack
                    && this.FromJoint.Column != this.ToJoint.Column)
                {
                    TimeCost = Calculator.CalculateTime(resourceMapper.RackThreshold,
                       Math.Abs(this.FromJoint.Column - this.ToJoint.Column) * resourceMapper.RackGap,
                       resourceMapper.PMaxSpeed,
                       resourceMapper.PAcceleration,
                       resourceMapper.PDeceleration);
                }
                //rack不同的DoWork类型，表示子车运行
                else if(this.FromJoint.Layer == this.ToJoint.Layer
                    && this.FromJoint.Rack != this.ToJoint.Rack
                    && this.FromJoint.Column == this.ToJoint.Column)
                {
                    TimeCost = Calculator.CalculateTime(resourceMapper.ColumnThreshold,
                       Math.Abs(this.FromJoint.Rack - this.ToJoint.Rack) * resourceMapper.ColumnGap,
                       resourceMapper.CMaxSpeed,
                       resourceMapper.CAcceleration,
                       resourceMapper.CDeceleration);
                }
                else if(this.FromJoint.Layer == this.ToJoint.Layer
                    && this.FromJoint.Rack == this.ToJoint.Rack
                    && this.FromJoint.Column == this.ToJoint.Column)
                {
                    TimeCost = 0;
                }
                else
                {
                    throw new Exception("Mission construction ERROR..." + from.ToString() + "::" + to.ToString());
                }
            }
        }

        public override string ToString()
        {
            return _debugger.StringAppend(MainTask.TaskName)
                .StringAppend("--> From: ").StringAppend(this.FromJoint.ToString())
                .StringAppend(", To: ").StringAppend(this.ToJoint.ToString())
                .StringAppend(", taking: ").StringAppend(this.TimeCost)
                .BuildString();
        }
    }

    public enum MissionType
    {
        Transmission,
        Lift,
        DoWork
    }

}

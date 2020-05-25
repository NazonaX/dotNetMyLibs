using System;
using System.Collections.Generic;
using System.Text;

namespace AnyTest.Schedule
{
    /// <summary>
    /// 使用String->Object映射的方式帮助寻找具体的Joint关键点的资源映射辅助类。
    /// </summary>
    public class ResourceMapper
    {
        private Dictionary<String, Joint> _resourceMapper = new Dictionary<string, Joint>();
        private Dictionary<Joint, ResourceStatus> _resourceStatus = null;
        private Dictionary<Joint, ResourceStatus> _resourceStatusBackup = new Dictionary<Joint, ResourceStatus>();
        public double RackGap { get; private set; }
        public double LayerGap { get; private set; }
        public double ColumnGap { get; private set; }
        public double LMaxSpeed { get; private set; }
        public double LAcceleration { get; private set; }
        public double LDeceleration { get; private set; }
        public double PMaxSpeed { get; private set; }
        public double PAcceleration { get; private set; }
        public double PDeceleration { get; private set; }
        public double CMaxSpeed { get; private set; }
        public double CAcceleration { get; private set; }
        public double CDeceleration { get; private set; }
        public double RackThreshold { get; private set; }
        public double ColumnThreshold { get; private set; }
        public double LayerThreshold { get; private set; }

        public ResourceMapper(double lgap, double rgap, double cgap, 
            double lms, double lacc, double ldec,
            double pms, double pacc, double pdec,
            double cms, double cacc, double cdec)
        {
            this.RackGap = rgap;
            this.ColumnGap = cgap;
            this.LayerGap = lgap;
            this.LMaxSpeed = lms;
            this.LAcceleration = lacc;
            this.LDeceleration = ldec;
            this.PMaxSpeed = pms;
            this.PAcceleration = pacc;
            this.PDeceleration = pdec;
            this.CMaxSpeed = cms;
            this.CAcceleration = cacc;
            this.CDeceleration = cdec;
            this.RackThreshold = Calculator.CalculateThreshold(PMaxSpeed, PAcceleration, PDeceleration);
            this.ColumnThreshold = Calculator.CalculateThreshold(CMaxSpeed, CAcceleration, CDeceleration);
            this.LayerThreshold = Calculator.CalculateThreshold(LMaxSpeed, LAcceleration, LDeceleration);
        }

        /// <summary>
        /// 通过string的key获取对应的Joint实体，不存在则返回null。
        /// </summary>
        public Joint GetJoint(String key)
        {
            return _resourceMapper.ContainsKey(key)
                ? _resourceMapper[key] : null;
        }

        /// <summary>
        /// 向mapper的dictionary中加入一对key-value
        /// </summary>
        public ResourceMapper AddJoint(String key, Joint joint)
        {
            if (_resourceMapper.ContainsKey(key))
                throw new Exception("The key: " + key + " has already been existed in the resource mapper.");
            _resourceMapper.Add(key, joint);
            if(!_resourceStatusBackup.ContainsKey(joint))
            {
                _resourceStatusBackup.Add(joint, new ResourceStatus(joint));
            }
            return this;
        }

        public ResourceStatus GetJointStatus(Joint joint)
        {
            if (!_resourceStatus.ContainsKey(joint))
                throw new Exception("No such joint resource in resource status dictionary...");
            return _resourceStatus[joint];
        }
        public void ResetResourceStatus()
        {
            _resourceStatus = new Dictionary<Joint, ResourceStatus>();
            foreach(KeyValuePair<Joint, ResourceStatus> kv in _resourceStatusBackup)
            {
                _resourceStatus.Add(kv.Key, kv.Value.Copy());
            }
        }

        public static ResourceMapper DefaultResourceMapper()
        {
            ResourceMapper resource = new ResourceMapper(lgap: 1.3, rgap: 0.8, cgap: 0.9,
                lms: 0.9, lacc: 1, ldec: 1,
                pms: 3, pacc: 2, pdec: 2,
                cms: 1.2, cacc: 1.6, cdec: 1.6);
            return resource;
        }
        

    }

    public class ResourceStatus
    {
        public int Layer { get; set; }
        public int Rack { get; set; }
        public int Column { get; set; }

        public ResourceStatus(Joint joint)
        {
            this.Layer = joint.Type == JointType.Lifter ? 0 : joint.Layer;
            this.Rack = joint.Rack;
            this.Column = joint.Column;
        }
        private ResourceStatus(ResourceStatus status)
        {
            this.Rack = status.Rack;
            this.Layer = status.Layer;
            this.Column = status.Column;
        }
        public ResourceStatus Copy()
        {
            return new ResourceStatus(this);
        }
    }
}

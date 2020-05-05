using System;
using System.Collections.Generic;
using System.Text;

namespace Session1_TPQR_MobileApp
{
    public class GlobalClass
    {
        public class User_Type
        {
            public int userTypeId { get; set; }
            public string userTypeName { get; set; }
        }
        public class User
        {
            public string userId { get; set; }
            public string userName { get; set; }
            public string userPw { get; set; }
            public int userTypeIdFK { get; set; }

        }
        public class CustomView
        {
            public string ResourceName { get; set; }
            public string ResourceType { get; set; }
            public int NumberOfSkills { get; set; }
            public string AllocatedSkills { get; set; }
            public string AvailableQuantity { get; set; }
        }
        public class Resource_Type
        {
            public int resTypeId { get; set; }
            public string resTypeName { get; set; }
        }
        public class Skill
        {

            public int skillId { get; set; }
            public string skillName { get; set; }
        }
        public class Resource
        {
            public int resId { get; set; }
            public string resName { get; set; }
            public int resTypeIdFK { get; set; }
            public int remainingQuantity { get; set; }
        }
        public class Resource_Allocation
        {
            public int allocId { get; set; }
            public int resIdFK { get; set; }
            public int skillIdFK { get; set; }

        }
    }
}

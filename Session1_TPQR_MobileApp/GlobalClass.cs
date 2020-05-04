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
    }
}

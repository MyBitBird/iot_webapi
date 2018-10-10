namespace IOT.Helper
{
    public class MyEnums{

        public enum ServiceStatus : short
        {
            ACTIVE = 1,
            DEACTICE = 2,
            DELETED = 3

        }

        public enum UserStatus : short
        {
            ACTIVE = 1,
            DEACTIVE = 2,
            DELETED = 3
        }

        public enum UserTypes
        {
            ADMIN = 1,
            DEVICE = 2
        }
    }
}
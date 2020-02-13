namespace IOT.Helper
{
    public class MyEnums{

        public enum ServiceStatus : short
        {
            Active = 1,
            Deactice = 2,
            Deleted = 3
        }
        public enum UserStatus : short
        {
            Active = 1,
            Deactive = 2,
            Deleted = 3
        }
        public enum UserTypes
        {
            Admin = 1,
            Device = 2
        }
    }
}
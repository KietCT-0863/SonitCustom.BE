namespace SonitCustom.BLL.Exceptions
{
    public class RoleNotFoundException : Exception
    {
        public int RoleId { get; }

        public RoleNotFoundException(int roleId)
            : base($"Không tìm thấy vai trò với ID: {roleId}")
        {
            RoleId = roleId;
        }
    }
} 
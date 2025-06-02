using System;

namespace SonitCustom.BLL.Exceptions
{
    public class RoleNameAlreadyExistsException : Exception
    {
        public string RoleName { get; }

        public RoleNameAlreadyExistsException(string roleName)
            : base($"Role với tên '{roleName}' đã tồn tại.")
        {
            RoleName = roleName;
        }

        public RoleNameAlreadyExistsException(string roleName, Exception innerException)
            : base($"Role với tên '{roleName}' đã tồn tại.", innerException)
        {
            RoleName = roleName;
        }
    }
} 
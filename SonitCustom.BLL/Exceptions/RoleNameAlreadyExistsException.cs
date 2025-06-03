namespace SonitCustom.BLL.Exceptions
{
    /// <summary>
    /// Ngoại lệ được ném ra khi cố gắng tạo hoặc đổi tên một role với tên đã tồn tại
    /// </summary>
    public class RoleNameAlreadyExistsException : Exception
    {
        /// <summary>
        /// Tên role đã tồn tại
        /// </summary>
        public string RoleName { get; }

        /// <summary>
        /// Khởi tạo một instance mới của <see cref="RoleNameAlreadyExistsException"/> với tên role được chỉ định
        /// </summary>
        /// <param name="roleName">Tên role đã tồn tại</param>
        public RoleNameAlreadyExistsException(string roleName)
            : base($"Role với tên '{roleName}' đã tồn tại.")
        {
            RoleName = roleName;
        }

        /// <summary>
        /// Khởi tạo một instance mới của <see cref="RoleNameAlreadyExistsException"/> với tên role và ngoại lệ bên trong được chỉ định
        /// </summary>
        /// <param name="roleName">Tên role đã tồn tại</param>
        /// <param name="innerException">Ngoại lệ bên trong</param>
        public RoleNameAlreadyExistsException(string roleName, Exception innerException)
            : base($"Role với tên '{roleName}' đã tồn tại.", innerException)
        {
            RoleName = roleName;
        }
    }
} 
﻿namespace RentCar.Application.Models.Permissions
{
    public class PermissionGroupListModel
    {
        public string Description { get; set; }

        public string GroupName { get; set; } = null!; // PermissionGroup.Name
        public List<PermissionListModel> Permissions { get; set; } = new List<PermissionListModel>();
    }
}

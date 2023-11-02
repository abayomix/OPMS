﻿using Repo_PMS.Models;

namespace Repo_PMS.Models
{
    public class UserRegistrationVM
    {
        public UserDetail UserDetail { get; set; }

        public List<RoleDetail>? LstRoles { get; set; }
        public List<Department>? Departments { get; set; }

        public List<Degination>? Deginations { get; set; }
        public List<UsersDLVM>? users { get; set; }




    }
}

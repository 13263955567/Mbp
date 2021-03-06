﻿using Mbp.Authentication.JwtBearer;
using Mbp.Ddd.Application.Mbp.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace EMS.Application.Contracts.AccountService.Dto
{
    public class LoginOutputDto : DtoBase
    {
        /// <summary>
        /// 用户姓名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 用户身份,超级管理员和非超级管理员
        /// </summary>
        public string Role { get; set; }

        /// <summary>
        /// 身份凭据
        /// </summary>
        public Jwt AccessToken { get; set; }

        /// <summary>
        /// 是否通过密码检查
        /// </summary>
        public bool IsPassPwdCheck { get; set; }

        /// <summary>
        /// 可见菜单 todo 设计成通用方案
        /// </summary>
        public List<string> Menus { get; set; } = new List<string>();
    }
}

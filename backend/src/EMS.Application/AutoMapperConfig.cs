﻿using AutoMapper;
using Mbp.Ddd.Application.Mbp.UI;
using Mbp.EntityFrameworkCore.PermissionModel;
using EMS.Application.Contracts.AccountService.Dto;
using EMS.Application.Contracts.Demo.Dto;
using EMS.Application.Contracts.LogService.Dto;
using EMS.Domain.DomainEntities.Demo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mbp.EntityFrameworkCore.Domain;

namespace EMS.Application
{
    /// <summary>
    /// object to object Map
    /// </summary>
    public static class AutoMapperConfig
    {
        public static IMapper CreateMapper()
        {
            // TO DO 在这里注册所有的类型映射cfg.CreateMap<TSource, TDestination>()
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Blog, BlogDto>();
                cfg.CreateMap<Post, PostDto>();

                cfg.CreateMap<BlogDto, Blog>();
                cfg.CreateMap<PostDto, Post>();

                // 用户映射
                cfg.CreateMap<UserInputDto, MbpUser>();
                cfg.CreateMap<MbpUser, UserOutputDto>();

                // 角色映射
                cfg.CreateMap<RoleInputDto, MbpRole>();
                cfg.CreateMap<MbpRole, RoleOutputDto>();

                // 菜单映射
                cfg.CreateMap<MenuInputDto, MbpMenu>();
                cfg.CreateMap<MbpMenu, MenuOutputDto>();

                // 日志映射
                cfg.CreateMap<LogInputDto, MbpOperationLog>();
                cfg.CreateMap<MbpOperationLog, LogOutInputDto>();

                // 路由映射
                cfg.CreateMap<MbpMenu, RouteOutputDto>();

                // 部门映射
                cfg.CreateMap<DeptInputDto, MbpDept>();
                cfg.CreateMap<MbpDept, DeptOutputDto>();

                // 岗位映射
                cfg.CreateMap<PositionInputDto, MbpPosition>();
                cfg.CreateMap<MbpPosition, PositionOutputDto>();
            });

            return config.CreateMapper();
        }
    }
}

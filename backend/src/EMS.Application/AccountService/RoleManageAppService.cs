﻿using AutoMapper;
using Mbp.AspNetCore.Mvc.Convention;
using Mbp.Core.Core;
using Mbp.Core.Modularity;
using Mbp.EntityFrameworkCore.PermissionModel;
using EMS.EntityFrameworkCore.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Mbp.Ddd.Application.System.Linq;
using Mbp.Ddd.Application.Mbp.UI;
using EMS.Application.Contracts.AccountService;
using EMS.Application.Contracts.AccountService.Dto;
using EMS.Application.Contracts.AccountService.DtoSearch;

namespace EMS.Application.AccountService
{
    [Authorize(Roles = "admin")]
    [AutoAop]
    [AutoWebApi]
    [Route("api/[controller]")]
    public class RoleManageAppService : IRoleManageAppService
    {
        private readonly IMapper _mapper = AutofacService.Resolve<IMapper>();

        private readonly DefaultDbContext _defaultDbContext = null;

        public RoleManageAppService(DefaultDbContext defaultDbContext)
        {
            _defaultDbContext = defaultDbContext;
        }

        /// <summary>
        /// 添加角色
        /// </summary>
        [HttpPost("AddRole")]
        public virtual int AddRole(RoleInputDto roleInputDto)
        {
            var role = _mapper.Map<MbpRole>(roleInputDto);

            _defaultDbContext.MbpRoles.Add(role);

            return _defaultDbContext.SaveChanges();
        }

        /// <summary>
        /// 更新角色
        /// </summary>
        /// <param name="roleInputDto"></param>
        /// <returns></returns>
        [HttpPut("UpdateRole")]
        public virtual int UpdateRole(RoleInputDto roleInputDto)
        {
            var role = _mapper.Map<MbpRole>(roleInputDto);

            _defaultDbContext.Attach(role);

            _defaultDbContext.MbpRoles.Update(role);

            return _defaultDbContext.SaveChanges();
        }

        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        [HttpDelete("DeleteRole")]
        public virtual int DeleteRole(int roleId)
        {
            var role = _defaultDbContext.MbpRoles.Where(r => r.Id == roleId).Include(r => r.RoleMenus).FirstOrDefault();
            _defaultDbContext.MbpRoles.Remove(role);

            return _defaultDbContext.SaveChanges();
        }

        /// <summary>
        /// 获取角色列表
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetRoles")]
        public virtual async Task<PagedList<RoleOutputDto>> GetRoles(SearchOptions<RoleSearchOptions> searchOptions)
        {
            int total = 0;

            // 分页查询 PageByAscending
            var roles = _defaultDbContext.MbpRoles.Include(u => u.RoleMenus).PageByAscending(searchOptions.PageSize, searchOptions.PageIndex, out total,
                (c) =>
            c.Name.Contains(searchOptions.Search.Name == null ? "" : searchOptions.Search.Name) &&
           (!string.IsNullOrEmpty(searchOptions.Search.SystemCode) ? c.SystemCode == searchOptions.Search.SystemCode : true), (c => c.Id)).ToList();

            // 返回列表分页数据
            return new PagedList<RoleOutputDto>()
            {
                Content = _mapper.Map<List<RoleOutputDto>>(roles),
                PageIndex = searchOptions.PageIndex,
                PageSize = searchOptions.PageSize,
                Total = total
            };
        }

        /// <summary>
        /// 配置角色功能
        /// </summary>
        [HttpPost("AddRoleMenus")]
        public virtual int AddRoleMenus(int roleId, List<int> menuIds)
        {
            // 查询已有的用户角色

            DeleteRoleMenus(roleId);

            List<MbpRoleMenu> mbpRoleMenus = new List<MbpRoleMenu>();

            foreach (var menuId in menuIds)
            {
                mbpRoleMenus.Add(new MbpRoleMenu() { RoleId = roleId, MenuId = menuId });
            }

            _defaultDbContext.MbpRoleMenus.AddRange(mbpRoleMenus);

            return _defaultDbContext.SaveChanges();
        }

        /// <summary>
        /// 删除角色功能关系,全部
        /// </summary>
        /// <param name="roleId"></param>
        [HttpDelete("DeleteRoleMenus")]
        public virtual int DeleteRoleMenus(int roleId)
        {
            var userRoles = _defaultDbContext.MbpRoleMenus
                .Where(rm => rm.RoleId == roleId).ToList();

            _defaultDbContext.MbpRoleMenus.RemoveRange(userRoles);

            return _defaultDbContext.SaveChanges();
        }

        /// <summary>
        /// 删除角色功能关系,单条
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="menuId"></param>
        [HttpDelete("DeleteRoleMenu")]
        public virtual int DeleteRoleMenu(int roleId, int menuId)
        {
            var roleMenu = _defaultDbContext.MbpRoleMenus
                .Where(rm => rm.RoleId == roleId && rm.MenuId == menuId)
                .FirstOrDefault();

            _defaultDbContext.MbpRoleMenus.Remove(roleMenu);

            return _defaultDbContext.SaveChanges();
        }

        /// <summary>
        /// 获取角色下的菜单
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        [HttpGet("GetRoleMenus")]
        public virtual List<RoleMenuOutputDto> GetRoleMenus(int roleId)
        {
            var menuRoles = _defaultDbContext.MbpRoleMenus.Where(rm => rm.RoleId == roleId)
                .Include(rm => rm.Menu)
                .ToList();

            List<RoleMenuOutputDto> roleMenuOutputDtos = new List<RoleMenuOutputDto>();

            menuRoles.ForEach(e =>
            {
                roleMenuOutputDtos.Add(new RoleMenuOutputDto()
                {
                    RoleId = e.RoleId,
                    MenuId = e.MenuId,
                    MenuCode = e.Menu.Code,
                    MenuLevel = e.Menu.Level,
                    MenuName = e.Menu.Name,
                    ParentId = e.Menu.ParentId,
                    Path = e.Menu.Path
                });
            });

            return roleMenuOutputDtos;
        }
    }
}

﻿using CharlieBackend.Business.Services.Interfaces;
using CharlieBackend.Core.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using System.Security.Claims;

namespace CharlieBackend.Business.Services
{
    /// <summary>
    /// Class that provides access to properties of the current user.
    /// </summary>
    public class CurrentUserService : ICurrentUserService
    {
        long _accountId;
        long _entityId;
        string _email;

        readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public long AccountId
        {
            get
            {
                if (_accountId == default)
                {
                    string accountIdString = _httpContextAccessor
                        .HttpContext
                        .User
                        ?.Claims
                        ?.SingleOrDefault(claim => claim.Type == "AccountId")
                        ?.Value;

                    if (!long.TryParse(accountIdString, out _accountId))
                    {
                        throw new UnauthorizedAccessException("Not authorized!");
                    }
                }

                return _accountId;
            }
        }

        /// <summary>
        /// Property that provides access to entity id of the current user.
        /// Corresponds to one of the following entities: 
        /// <list type="bullet">
        /// <item><see cref="Student"/></item>
        /// <item><see cref="Secretary"/></item>
        /// <item><see cref="Mentor"/></item>
        /// </list>
        /// </summary>
        public long EntityId
        {
            get
            {
                if (_entityId == default)
                {
                    string entityIdString = _httpContextAccessor
                        .HttpContext
                        .User
                        ?.Claims
                        ?.SingleOrDefault(claim => claim.Type == "Id")
                        ?.Value;

                    if (!long.TryParse(entityIdString, out _entityId))
                    {
                        throw new UnauthorizedAccessException("Not authorized!");
                    }
                }

                return _entityId;
            }
        }

        public string Email
        {
            get
            {
                if (_email is null)
                {
                    _email = _httpContextAccessor
                        .HttpContext
                        .User
                        ?.Claims
                        ?.SingleOrDefault(claim => claim.Type == "Email")
                        ?.Value;

                    if (_email is null)
                    {
                        throw new UnauthorizedAccessException("Not authorized!");
                    }
                }

                return _email;
            }
        }

        public UserRole Role
        {
            get
            {
                string roleString = _httpContextAccessor
                    .HttpContext
                    .User
                    ?.Claims
                    ?.SingleOrDefault(claim => claim.Type == ClaimsIdentity.DefaultRoleClaimType)
                    ?.Value;

                UserRole role;

                if (!Enum.TryParse<UserRole>(roleString, out role))
                {
                    throw new UnauthorizedAccessException("Not authorized!");
                }

                return role;
            }
        }
    }
}

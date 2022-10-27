﻿using AutoMapper;
using AutoMapper.QueryableExtensions;
using DDStudy2022.Api.Interfaces;
using DDStudy2022.Api.Models;
using DDStudy2022.DAL;
using DDStudy2022.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace DDStudy2022.Api.Services;

public class UserService : IUserService
{
    private readonly IMapper _mapper;
    private readonly DataContext _context;

    public UserService(IMapper mapper, DataContext context)
    {
        _mapper = mapper;
        _context = context;
    }

    public async Task CreateUser(CreateUserModel model)
    {
        var dbUser = _mapper.Map<User>(model);
        await _context.Users.AddAsync(dbUser);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<UserModel>> GetAllUsers()
    {
        return await _context.Users.AsNoTracking()
            .ProjectTo<UserModel>(_mapper.ConfigurationProvider)
            .ToListAsync();
    }

    public async Task<bool> FindByMail(CreateUserModel user)
    {
        var isExist = await _context.Users.AnyAsync(it => it.Email == user.Email);
        return isExist;
    }
}
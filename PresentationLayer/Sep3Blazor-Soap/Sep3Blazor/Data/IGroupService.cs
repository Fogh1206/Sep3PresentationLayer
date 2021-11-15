﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Sep3Blazor.Model;

namespace Sep3Blazor.Data
{
    public interface IGroupService
    {
        
        public Task<IList<Note>> GetNoteList(String s);
        
        public Task<IList<Group>> GetGroupList(String s);
        
        public Task<IList<Group>> AddGroup(String s);

        public  Task DeleteGroup(string s);

        public Task<User> ValidateLogin(string username, string password);
        public Task<User> RegisterUser(User user);



    }
}
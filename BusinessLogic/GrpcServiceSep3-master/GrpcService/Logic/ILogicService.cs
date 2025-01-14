﻿using System.Threading.Tasks;
using Grpc.Core;

namespace GrpcService.Logic
{
    public interface ILogicService
    {
        // Note
        public Task<Reply> PostNote(NoteRequest request, ServerCallContext context);
        public Task<Reply> PutNote(NoteRequest request, ServerCallContext context);
        public Task<Reply> DeleteNote(Request request, ServerCallContext context);
        public Task<Reply> GetNoteList(Request request, ServerCallContext context);

        // Group
        public Task<Reply> PostGroup(PostGroupRequest request, ServerCallContext context);

        public Task<Reply> DeleteGroup(Request request, ServerCallContext context);

        public Task<Reply> GetGroupList(Request request, ServerCallContext context);

        // User
        public Task<RegisterReply> RegisterUser(RegisterRequest request, ServerCallContext context);
        public Task<Reply> ValidateUser(Request request, ServerCallContext context);
        public Task<Reply> EditUser(EditUserRequest request, ServerCallContext context);

        // Invitations
        public Task<Reply> GetInvitationList(Request request, ServerCallContext context);
        public Task<Reply> PostInvitation(PostInvitationRequest request, ServerCallContext context);
        public Task<Reply> DeleteInvitation(Request request, ServerCallContext context);


        public Task<Reply> GetGroupMemberList(Request request, ServerCallContext context);


        public Task<Reply> AddGroupMember(AddGroupMemberRequest request, ServerCallContext context);

        public Task<Reply> DeleteGroupMember(UserRequest request, ServerCallContext context);

        public Task<Reply> DeleteUser(UserRequest request, ServerCallContext context);
        public Task<Reply> GetUserList(GetUserRequest request, ServerCallContext context);
        public Task<Reply> LeaveGroup(DeleteGroupMemberRequest request, ServerCallContext context);
    }
}
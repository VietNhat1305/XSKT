using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using System.Collections.Generic;
using XoSoKienThiet.Constants;
using XoSoKienThiet.Exceptions;
using XoSoKienThiet.Extensions;
using XoSoKienThiet.Helpers;
using XoSoKienThiet.Installers;
using XoSoKienThiet.Interfaces.Core;
using XoSoKienThiet.Models.Core;
using XoSoKienThiet.Models.PagingParam;
using XoSoKienThiet.ViewModels;

namespace XoSoKienThiet.Services.Core
{
    public class UserService : BaseService, IUserService
    {
        private DataContext _context;
        private BaseMongoDb<User, string> BaseMongoDb;
        IMongoCollection<User> _collectionUser;
        protected ProjectionDefinition<User, BsonDocument> projectionDefinition = Builders<User>.Projection
           .Exclude("ModifiedAt")
           .Exclude("CreatedBy")
           .Exclude("ModifiedBy")
           .Exclude("IsDeleted")
           .Exclude("CreatedAtString")
           .Exclude("PasswordSalt")
           .Exclude("PasswordHash")
           .Exclude("Sort")
           .Exclude("UnsignedName");

        public UserService(
            DataContext context, 
            IHttpContextAccessor contextAccessor) :
            base(context, contextAccessor)
        {
            _context = context;
            BaseMongoDb = new BaseMongoDb<User, string>(_context.USERS);
            _collectionUser = context.USERS;
        }
        
        
        
        public async Task<User> GetByUserName(string userName)
        {
            return await _context.USERS.Find(x => x.UserName == userName && x.IsDeleted != true).FirstOrDefaultAsync();
        }

        public async Task<User> Create(User model)
        {
            if (model == default)
                throw new ResponseMessageException().WithException(DefaultCode.ERROR_STRUCTURE);

            if (FormatterString.HasDiacritics(model.UserName))
            {
                throw new ResponseMessageException().WithCode(DefaultCode.CREATE_FAILURE)
                   .WithMessage("UserName không được có dấu.");
            }

            var checkName = _context.USERS.Find(x => x.UserName == model.UserName && x.IsDeleted != true)
                .FirstOrDefault();

            if (checkName != default)
            {
                throw new ResponseMessageException().WithCode(DefaultCode.CREATE_FAILURE)
                     .WithMessage($"Tên tài khoản {checkName.UserName} đã tồn tại.");
            }

            var entity = new User
            {
                UserName = model.UserName,
                Name = model.UserName,
                FullName = model.FullName,
                Phone = model.Phone,
                Email = model.Email,
                UnitRole = model.UnitRole,
                IsVerified = model.IsVerified,
                IsSyncPasswordSuccess = model.IsSyncPasswordSuccess,
                IsActived = model.IsActived,
                CreatedAt = DateTime.Now,
                ModifiedAt = DateTime.Now
            };


            byte[] passwordHash, passwordSalt;
            if (string.IsNullOrEmpty(model.Password))
            {
                model.Password = "DongThap@123";
            }
            PasswordExtensions.CreatePasswordHash(model.Password, out passwordHash, out passwordSalt);
            entity.PasswordHash = passwordHash;
            entity.PasswordSalt = passwordSalt;

            var result = await BaseMongoDb.CreateAsync(entity);
            if (result.Entity.Id == default || !result.Success)
            {
                throw new ResponseMessageException().WithException(DefaultCode.CREATE_FAILURE);
            }
            return entity;
        }

        public async Task<User> Update(User model)
        {
            if (model == default)
                throw new ResponseMessageException().WithException(DefaultCode.ERROR_STRUCTURE);
            var entity = _context.USERS.Find(x => x.Id == model.Id).FirstOrDefault();

            if (entity == default)
                throw new ResponseMessageException().WithException(DefaultCode.DATA_NOT_FOUND);

            var checkName = _context.USERS
                .Find(x => x.Id != model.Id && x.UserName == model.UserName && x.IsDeleted != true).FirstOrDefault();

            if (checkName != default)
                throw new ResponseMessageException().WithCode(DefaultCode.DATA_EXISTED)
                    .WithMessage("Tên tài khoản đã tồn tại");

            entity.UserName = model.UserName;
            entity.FullName = model.FullName;
            entity.Phone = model.Phone;
            entity.Email = model.Email;
            entity.UnitRole = model.UnitRole;
            entity.ModifiedAt = DateTime.Now;
            entity.IsVerified = model.IsVerified;
            entity.IsSyncPasswordSuccess = model.IsSyncPasswordSuccess;
            entity.IsActived = model.IsActived;
            if (!string.IsNullOrEmpty(model.Password))
            {
                byte[] passwordHash, passwordSalt;
                PasswordExtensions.CreatePasswordHash(model.Password, out passwordHash, out passwordSalt);
                entity.PasswordHash = passwordHash;
                entity.PasswordSalt = passwordSalt;
            }

            var result = await BaseMongoDb.UpdateAsync(entity);
            if (!result.Success)
            {
                throw new ResponseMessageException().WithException(DefaultCode.UPDATE_FAILURE);
            }
            
            return entity;
        }

        public async Task<User> ChangeProfile(User model)
        {
            if (model == default)
            {
                throw new ResponseMessageException().WithException(DefaultCode.ERROR_STRUCTURE);
            }

            var entity = _context.USERS.Find(x => x.Id == model.Id && !x.IsDeleted).FirstOrDefault();
            if (entity == default)
            {
                throw new ResponseMessageException().WithException(DefaultCode.DATA_NOT_FOUND);
            }


            entity.FullName = model.FullName;
            entity.Phone = model.Phone;
            entity.Email = model.Email;
            entity.Avatar = model.Avatar;

            var result = await BaseMongoDb.UpdateAsync(entity);
            if (!result.Success)
            {
                throw new ResponseMessageException().WithException(DefaultCode.UPDATE_FAILURE);
            }
            
            return entity;
        }

        public async Task<User> ChangePassword(UserVM model)
        {
            if (model == default)
                throw new ResponseMessageException().WithException(DefaultCode.ERROR_STRUCTURE);
            var entity = _context.USERS.Find(x => x.UserName == model.UserName && !x.IsDeleted).FirstOrDefault();
            if (entity == default)
                throw new ResponseMessageException().WithException(DefaultCode.DATA_NOT_FOUND);
            if (!string.IsNullOrEmpty(model.Password))
            {
                byte[] passHash, passSalt;
                passHash = entity.PasswordHash;
                passSalt = entity.PasswordSalt;
                var pass = PasswordExtensions.VerifyPasswordHash(model.Password, passHash, passSalt);
                if (pass != true)
                {
                    throw new ResponseMessageException().WithCode(DefaultCode.UPDATE_FAILURE)
                        .WithMessage("Mật khẩu không chính xác");
                }
                else
                {
                    if (model.newPass == model.confirmPass)
                    {
                        byte[] passwordHash, passwordSalt;
                        PasswordExtensions.CreatePasswordHash(model.newPass, out passwordHash, out passwordSalt);
                        entity.PasswordHash = passwordHash;
                        entity.PasswordSalt = passwordSalt;
                    }
                }
            }

            var result = await BaseMongoDb.UpdateAsync(entity);
            if (!result.Success)
            {
                throw new ResponseMessageException().WithCode(DefaultCode.UPDATE_FAILURE)
                    .WithMessage("Đổi mật khẩu thất bại.");
            }
            
            return entity;
        }
        public async Task<dynamic> GetPaging(PagingParam pagingParam)
        {
            try
            {
                PagingModel<dynamic> result = new PagingModel<dynamic>();
                var builder = Builders<User>.Filter;
                var filter = builder.Empty;
                filter = builder.And(filter, builder.Eq("IsDeleted", false));
               
                filter = builder.And(filter,
                builder.Where(x => !x.UserName.Equals("supperadmin")));
              
                if (!String.IsNullOrEmpty(pagingParam.Content))
                {
                    filter = builder.And(filter,
                        (builder.Regex("UnsignedName", FormatterString.ConvertToUnsign(pagingParam.Content)) |
                         builder.Regex("Code", pagingParam.Content)
                        ));
                }

                if (pagingParam.Level != null && pagingParam.Level >= 0)
                {
                    filter = builder.And(filter,
                        builder.Eq("Level", pagingParam.Level)
                        );
                }

                if (pagingParam.IdDonViCha != null && !pagingParam.IdDonViCha.Equals(""))
                {
                    filter = builder.And(filter,
                        builder.Eq("IdDonViCha", pagingParam.IdDonViCha)
                    );
                }
               
                result.TotalRows = await _collectionUser.CountDocumentsAsync(filter);

                string sortBy = pagingParam.SortBy != null ? FormatterString.HandlerSortBy(pagingParam.SortBy) : "CreatedAt";

                var list = await _collectionUser.Find(filter)
                    .Sort(pagingParam.SortDesc
                        ? Builders<User>
                            .Sort.Descending(sortBy)
                        : Builders<User>
                            .Sort.Ascending(sortBy))
                    .Skip(pagingParam.Skip)
                    .Limit(pagingParam.Limit)
                    .Project(projectionDefinition)
                    .ToListAsync();

                result.Data = list.Select(x => BsonSerializer.Deserialize<User>(x)).ToList();

                return result;
            }
            catch (ResponseMessageException e)
            {
                new ResultMessageResponse().WithCode(e.ResultCode)
                    .WithMessage(e.ResultString);
            }
            catch (Exception e)
            {
                if (e.Message.Contains("is not a valid 24 digit hex string."))
                {
                    throw new ResponseMessageException().WithException(DefaultCode.ID_NOT_CORRECT_FORMAT);
                }

                if (e.Message.Contains("ObjectSerializer does not support BSON type 'Binary'."))
                {
                    throw new ResponseMessageException().WithException(DefaultCode.SORT_BY_NOT_EXIST);
                }
                throw new ResponseMessageException().WithCode(DefaultCode.EXCEPTION).WithMessage(e.Message);
            }
            return null;
        }

    }
}

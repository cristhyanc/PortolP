using System;
using System.Collections.Generic;
using System.Text;

namespace PortolWeb.Entities
{
    public interface IUnitOfWork
    {
        IRepositoryBasey<User> UserRepository { get; }

        void SaveChanges();
    }


}

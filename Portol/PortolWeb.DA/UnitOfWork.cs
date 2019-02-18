using PortolWeb.DA.Repositories;
using PortolWeb.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PortolWeb.DA
{
   public  class UnitOfWork: IUnitOfWork
    {
        private readonly DataContext _context;
        private IRepositoryBasey<User> _userRepository;

        public UnitOfWork(DataContext context)
        {
            _context = context;
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public IRepositoryBasey<User> UserRepository
        {
            get
            {
                return _userRepository = _userRepository ?? new RepositoryBasey<User>(_context);
            }
        }

    }
}

using BookStore.Data;
using BookStore.Models;

namespace BookStore.Services
{
    public class UserService
    {
        private readonly AppDbContext _dbContext;

        public UserService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<User> GetAll()
        {
            return _dbContext.Users.ToList();
        }

        public User? Get(int id)
        {
            return _dbContext.Users.FirstOrDefault(u => u.Id == id);
        }

        public void Add(User user)
        {
            _dbContext.Users.Add(user);
            _dbContext.SaveChanges();
        }

        public void Delete(int id)
        {
            var user = _dbContext.Users.FirstOrDefault(u => u.Id == id);
            if (user != null)
            {
                _dbContext.Users.Remove(user);
                _dbContext.SaveChanges();
            }
        }

        public void Update(User user)
        {
            var existingUser = _dbContext.Users.FirstOrDefault(u => u.Id == user.Id);
            if (existingUser != null)
            {
                existingUser.Username = user.Username;
                existingUser.Email = user.Email;
                existingUser.Password = user.Password;
                existingUser.Phone = user.Phone;
                existingUser.Address = user.Address;
                existingUser.Role = user.Role;

                _dbContext.SaveChanges();
            }
        }

        public User? GetUserById(int userId)
        {
            return _dbContext.Users.FirstOrDefault(u => u.Id == userId);
        }

        public void UpdateUser(User user)
        {
            var existingUser = _dbContext.Users.FirstOrDefault(u => u.Id == user.Id);
            if (existingUser != null)
            {
                existingUser.Email = user.Email;
                existingUser.Password = user.Password;
                existingUser.Phone = user.Phone;
                existingUser.Address = user.Address;

                _dbContext.SaveChanges();
            }
        }

        public User? Authenticate(string username, string password)
        {
            return _dbContext.Users.FirstOrDefault(u => u.Username == username && u.Password == password);
        }

        public bool UsernameExists(string username)
        {
            return _dbContext.Users.Any(u => u.Username == username);
        }

        public bool EmailExists(string email)
        {
            return _dbContext.Users.Any(u => u.Email == email);
        }

        public bool IsRoleValid(string role)
        {
            string[] validRoles = { "Buyer", "Seller", "Admin" };
            return validRoles.Contains(role);
        }
    }
}

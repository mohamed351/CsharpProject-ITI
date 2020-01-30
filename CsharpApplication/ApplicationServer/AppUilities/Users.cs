using AppUilities.UserGameDelegates;
using AppUilities.UserGameExceptions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppUilities
{
    /// <summary>
    /// Describing the User That is connected on our server
    /// </summary>
    public class Users
    {
        public event UserOnlineHandler OnOnlineUser;
        public event UserOfflineHandler OnUserOffline;
        /// <summary>
        /// Information about the User
        /// </summary>
        /// <param name="id">GUID for User </param>
        /// <param name="name">The Name of User </param>
        /// <param name="userName">The UserName because of login </param>
        /// <param name="password">The Password because of login</param>
        /// <param name="status">The Status Offline 0 and Online 1 </param>
        /// <param name="pictureNumber">Picture Number</param>
        public Users(Guid id, string name, string userName, string password, string status, int pictureNumber)
            : this()
        {

            this.ID = id;
            this.Name = name;
            this.UserName = userName;
            this.Password = password;
            this.PictureNumber = pictureNumber;

        }
        /// <summary>
        /// This is constructor made if any one want to use object Initilizer 
        /// Please Please Write All information about user please or See Documentation
        /// </summary>
        public Users()
        {



        }

        /// <summary>
        /// Describe the ID of User It is Unique ID
        /// </summary>
        public Guid ID { get; set; }
        /// <summary>
        /// Describe the Name of Name 
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Describe the UserName of User to make login
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// Describe the Password  of user to make login
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// Describe the Age of user
        /// </summary>
        public int Age { get; set; }
        /// <summary>
        /// Describe The Login Status 
        /// </summary>
        public byte status { get; set; }
        /// <summary>
        /// Describle The Number of Picture there is a picture Named
        /// 1.png
        /// 2.png 
        /// in the Project 
        /// </summary>
        public int PictureNumber { get; set; }
        /// <summary>
        /// Print the Info About User with our Text Format
        /// </summary>
        /// <returns> Text formate that is Saved in Text File</returns>
        public override string ToString()
        {
            return $"{ID},{Name},{UserName},{Password},{Age},{PictureNumber} ,{status}";
        }
        /// <summary>
        /// Get All users of Project
        /// </summary>
        /// <returns>List of Users </returns>
        public static List<Users> GetUsers()
        {
            string[] vs = File.ReadAllLines("Users.txt");

            List<Users> users = new List<Users>();
            foreach (var item in vs)
            {
                string[] items = item.Split(',');
                Users user = new Users();
                user.ID = Guid.Parse(items[0]);
                user.Name = items[1];
                user.UserName = items[2];
                user.Password = items[3];
                user.Age = Convert.ToInt32(items[4]);
                user.PictureNumber = Convert.ToInt32(items[5]);
                user.status = Convert.ToByte(items[6]);
                users.Add(user);

            }

            return users;
        }
        /// <summary>
        /// Check the User Is Exist or not
        /// </summary>
        /// <param name="userName"> UserName should be Unique</param>
        /// <returns> True if user Found  </returns>
        public static bool UserNameIsExist(string userName)
        {
            bool isExist = false;
            string[] AllText = File.ReadAllLines("Users.txt");
            foreach (var item in AllText)
            {
                var element = item.Split(',');
                if (!(element.Length < 2))
                {
                    if (element[2] == userName)
                    {
                        isExist = true;
                    }
                }
            }

            return isExist;

        }
        /// <summary>
        /// To Add New User
        /// </summary>
        /// <param name="users">Add New User</param>
        public static void AddUser(Users users)
        {
            if (!UserNameIsExist(users.UserName))
            {
                using (FileStream fileStream = new FileStream("Users.txt", FileMode.Open, FileAccess.ReadWrite))
                {

                    fileStream.Seek(0, SeekOrigin.End);
                    byte[] array = Encoding.Default.GetBytes(users.ToString());
                    var f = fileStream.ReadByte();

                    byte[] newline = Encoding.Default.GetBytes(Environment.NewLine);
                    fileStream.Write(newline, 0, newline.Length);


                    fileStream.Write(array, 0, array.Length);
                    fileStream.Close();
                }
            }
            else
            {
                throw new UserExistException("The User Is Found Please Write Another User");

            }

        }
        /// <summary>
        /// Get User throw the GUID
        /// </summary>
        /// <param name="guid"> Guid The Describe the User ID </param>
        /// <returns> The current User that has Guid</returns>
        public static Users GetUser(Guid guid)
        {
            Users newUser = null;
            string[] users = File.ReadAllLines("Users.txt");
            foreach (var item in users)
            {
                string[] infos = item.Split(',');
                Guid id = Guid.Parse(infos[0]);
                if (guid == id)
                {

                    newUser = new Users();
                    newUser.ID = Guid.Parse(infos[0]);
                    newUser.Name = infos[1];
                    newUser.UserName = infos[2];
                    newUser.Password = infos[3];
                    newUser.Age = Convert.ToInt32(infos[4]);
                    newUser.PictureNumber = Convert.ToInt32(infos[5]);
                    newUser.status = Convert.ToByte(infos[6]);

                }
            }
            return newUser;

        }

        /// <summary>
        /// If you write a userName and Password right it will return The User 
        /// and If you didn't write a userName and Password wrong It Will Give the 
        /// User referenced to null if you want to know the userName and Password 
        /// correct please check null
        /// </summary>
        /// <param name="userName"> UserName : of login in Text File</param>
        /// <param name="Password"> Password: of login in Text File </param>
        /// <returns> The User if Exist and null if doesn't Exist</returns>
        public static Users LoginUser(string userName, string Password)
        {
           var  user = GetUsers()
                .SingleOrDefault(a => a.UserName == userName && a.Password == Password);
            if (user.status == 0)
            {
                user.status = 1;
                if(user.OnOnlineUser != null)
                {
                    user.OnOnlineUser(user);
                }
               
            }
            else
            {
                //make your own Exception
                throw new Exception("The User is already connected");
            }

            return user;

        }
        /// <summary>
        /// Get the User
        /// </summary>
        /// <param name="delegate"> The Condidition  function on Single Or Default</param>
        /// <returns></returns>
        public static Users GetUserUsingLinq(Func<Users, bool> @delegate)
        {
            return GetUsers().SingleOrDefault(@delegate);

        }
        /// <summary>
        /// Get Users 
        /// </summary>
        /// <param name="delegate"> The Condidition  function on Where</param>
        /// <returns>The List Of User base on Condition </returns>
        public static List<Users> GetUsersUsingLinq(Func<Users, bool> @delegate)
        {
            return GetUsers().Where(@delegate).ToList();

        }

        /// <summary>
        /// Print This is Kind Of User
        /// </summary>
        /// <returns>The UserInformation </returns>
        public virtual string PrintUser()
        {
            return $"ID:{this.ID}\nName:{this.Name}\nUserName:{this.UserName}\nPassword:{this.Password}\nAge:{this.Age}\nPictureNumber:{this.PictureNumber}\nStatus:{this.status}";
        }

       

        public void LogOut()
        {
            this.status = 0;
            
            
        }

      

        







    }
}

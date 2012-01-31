using System.Collections.Generic;

namespace JabbrPhone.Models
{
    public class RoomModel
    {
        public string Name { get; set; }
        public int Count { get; set; }
        public bool Private { get; set; }
        public List<UserModel> Users { get; set; }
        public List<string> Owners { get; set; }
        public List<MessageModel> RecentMessages { get; set; }

        public void CheckOwners()
        {
            if (Users == null) return;

            foreach (var user in Users)
            {
                user.IsOwner = Owners.Contains(user.Name);
            }
        }
    }
}

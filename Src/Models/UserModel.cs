using Amazon.DynamoDBv2.DataModel;

namespace DebtTrack.Models;


[DynamoDBTable("Users")]
public class UserModel
{
        [DynamoDBHashKey] 
        public string UserId { get; set; } = Guid.NewGuid().ToString();

        [DynamoDBProperty]
        public string Name { get; set; }

        [DynamoDBProperty]
        public string Email { get; set; }

        [DynamoDBProperty]
        public string Password { get; set; } 
    
}
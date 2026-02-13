using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;

namespace DebtTrack.Setup
{
    public class DynamoDbSetup
    {
        private readonly IAmazonDynamoDB _client;

        public DynamoDbSetup(IAmazonDynamoDB client)
        {
            _client = client;
        }

        public async Task CreateTablesAsync()
        {
            var tables = new List<CreateTableRequest>
            {
     
                new CreateTableRequest
                {
                    TableName = "Users",
                    AttributeDefinitions = new List<AttributeDefinition>
                    {
                        new AttributeDefinition("UserId", ScalarAttributeType.S)
                    },
                    KeySchema = new List<KeySchemaElement>
                    {
                        new KeySchemaElement("UserId", KeyType.HASH)
                    },
                    BillingMode = BillingMode.PAY_PER_REQUEST
                },

       
                new CreateTableRequest
                {
                    TableName = "Debts",
                    AttributeDefinitions = new List<AttributeDefinition>
                    {
                        new AttributeDefinition("UserId", ScalarAttributeType.S),   
                        new AttributeDefinition("DebtId", ScalarAttributeType.S)   
                    },
                    KeySchema = new List<KeySchemaElement>
                    {
                        new KeySchemaElement("UserId", KeyType.HASH),
                        new KeySchemaElement("DebtId", KeyType.RANGE)
                    },
                    BillingMode = BillingMode.PAY_PER_REQUEST
                },

        
                new CreateTableRequest
                {
                    TableName = "Installments",
                    AttributeDefinitions = new List<AttributeDefinition>
                    {
                        new AttributeDefinition("UserId", ScalarAttributeType.S),          
                        new AttributeDefinition("InstallmentId", ScalarAttributeType.S)     
                    },
                    KeySchema = new List<KeySchemaElement>
                    {
                        new KeySchemaElement("UserId", KeyType.HASH),
                        new KeySchemaElement("InstallmentId", KeyType.RANGE)
                    },
                    BillingMode = BillingMode.PAY_PER_REQUEST
                },


                new CreateTableRequest
                {
                    TableName = "Payments",
                    AttributeDefinitions = new List<AttributeDefinition>
                    {
                        new AttributeDefinition("UserId", ScalarAttributeType.S),  
                        new AttributeDefinition("PaymentId", ScalarAttributeType.S) 
                    },
                    KeySchema = new List<KeySchemaElement>
                    {
                        new KeySchemaElement("UserId", KeyType.HASH),
                        new KeySchemaElement("PaymentId", KeyType.RANGE)
                    },
                    BillingMode = BillingMode.PAY_PER_REQUEST
                }
            };

            foreach (var table in tables)
            {
                try
                {
                    var existing = await _client.ListTablesAsync();
                    if (!existing.TableNames.Contains(table.TableName))
                    {
                        var response = await _client.CreateTableAsync(table);
                        Console.WriteLine($"✅ Tabela '{response.TableDescription.TableName}' criada com sucesso!");
                    }
                    else
                    {
                        Console.WriteLine($"⚠️ Tabela '{table.TableName}' já existe.");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"❌ Erro criando {table.TableName}: {ex.Message}");
                }
            }
        }


        public async Task DropTablesAsync()
{
    var tableNames = new[] { "Debts", "Installments", "Payments" };

    foreach (var tableName in tableNames)
    {
        try
        {
            var existing = await _client.ListTablesAsync();
            if (existing.TableNames.Contains(tableName))
            {
                await _client.DeleteTableAsync(tableName);
                Console.WriteLine($"🗑️ Tabela '{tableName}' deletada com sucesso!");
            }
            else
            {
                Console.WriteLine($"⚠️ Tabela '{tableName}' não existe, nada a deletar.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Erro ao deletar {tableName}: {ex.Message}");
        }
    }
}

    }
    
}

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
                        new AttributeDefinition("DebtId", ScalarAttributeType.S)
                    },
                    KeySchema = new List<KeySchemaElement>
                    {
                        new KeySchemaElement("DebtId", KeyType.HASH)
                    },
                    BillingMode = BillingMode.PAY_PER_REQUEST
                },
                new CreateTableRequest
                {
                    TableName = "Installments",
                    AttributeDefinitions = new List<AttributeDefinition>
                    {
                        new AttributeDefinition("InstallmentId", ScalarAttributeType.S)
                    },
                    KeySchema = new List<KeySchemaElement>
                    {
                        new KeySchemaElement("InstallmentId", KeyType.HASH)
                    },
                    BillingMode = BillingMode.PAY_PER_REQUEST
                },
                new CreateTableRequest
                {
                    TableName = "Payments",
                    AttributeDefinitions = new List<AttributeDefinition>
                    {
                        new AttributeDefinition("PaymentId", ScalarAttributeType.S)
                    },
                    KeySchema = new List<KeySchemaElement>
                    {
                        new KeySchemaElement("PaymentId", KeyType.HASH)
                    },
                    BillingMode = BillingMode.PAY_PER_REQUEST
                }
            };

            foreach (var table in tables)
            {
                try
                {
                    // Verifica se já existe antes de criar
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
    }
}

#Basket Service
dapr run --app-id basket-service --app-port "5002" --dapr-http-port 3602 --components-path "./components" -- dotnet run --project ./sdu.bachelor.microservice.basket/sdu.bachelor.microservice.basket.csproj


#Catalog Service
dapr run --app-id catalog-service --app-port "5001" --dapr-http-port 3601 --components-path "./components" -- dotnet run --project ./sdu.bachelor.microservice.catalog/sdu.bachelor.microservice.catalog.csproj
#Publish events to On_Order_Added_To_Basket Topic:
dapr publish --publish-app-id catalog-service --pubsub kafka-commonpubsub -t On_Order_Added_To_Basket  -d '{"orderId": "100"}'

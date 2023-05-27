import time
import random
import uuid
from locust import FastHttpUser, task, between

products = ["ab0f5a1f-9b48-4862-8e6a-bced8d20558e", "7201fd50-25b9-4b7d-99a7-b367b73222f8", "d4b1d999-862d-4cf9-bcb7-b79de08768b9"]


class QuickstartUser(FastHttpUser):
    wait_time = between(0.1, 0.1)

    @task
    def reserve_products(self):
        self.client.post("/api/v1/basket/reserve", json={"CustomerId": str(uuid.uuid4()), "Products": [
            {"Quantity": 1, "Price": 11000.0, "ProductId": random.choice(products)}]})

    # @task(3)
    # def view_items(self):
    #     for item_id in range(10):
    #         self.client.get(f"/item?id={item_id}", name="/item")
    #         time.sleep(1)

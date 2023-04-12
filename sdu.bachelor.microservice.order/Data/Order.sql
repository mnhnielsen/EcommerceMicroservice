CREATE TABLE Orders(
	OrderId VARCHAR(36) PRIMARY KEY,
	CustomerId VARCHAR(36) NOT NULL,
	OrderStatus VARCHAR(20) NOT NULL

);

CREATE TABLE OrderItems(
	Id VARCHAR(36) NOT NULL,
	OrderId VARCHAR(36) NOT NULL,
	ProductId VARCHAR(36) NOT NULL,
	Price DOUBLE NOT NULL,
	Quantity INT NOT NULL,

	FOREIGN KEY (OrderId) REFERENCES Orders(OrderId)

);

INSERT INTO Orders VALUES("3dea5bb2-9f8d-4c7f-9a97-e00de86f546d", "33e7784d-e548-4bb5-9e1c-94a205a3d49b", "Reserved");
INSERT INTO Orders VALUES("0b98b2dc-5221-4ad9-9f15-80a905c9a48b", "33e7784d-e548-4bb5-9e1c-94a205a3d49b", "Reserved");


INSERT INTO OrderItems VALUES("2fcdd249-2dd0-4581-81ba-afd824e2c74c", "3dea5bb2-9f8d-4c7f-9a97-e00de86f546d", "7201fd50-25b9-4b7d-99a7-b367b73222f8", 10000.0, 1);
INSERT INTO OrderItems VALUES("b63955cf-6f85-432f-b0dc-11f122b70c46", "3dea5bb2-9f8d-4c7f-9a97-e00de86f546d", "ab0f5a1f-9b48-4862-8e6a-bced8d20558e", 11000.0, 1);


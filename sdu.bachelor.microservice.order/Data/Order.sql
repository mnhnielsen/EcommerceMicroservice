CREATE TABLE Orders(
	Id VARCHAR(36) PRIMARY KEY,
	CustomerId VARCHAR(36) NOT NULL,
	OrderStatus VARCHAR(20) NOT NULL

);

CREATE TABLE OrderItems(
	Id VARCHAR(36) NOT NULL,
	OrderId VARCHAR(36) NOT NULL,
	ProductId VARCHAR(36) NOT NULL,
	Price DOUBLE NOT NULL,
	Quantity INT NOT NULL,

	FOREIGN KEY (OrderId) REFERENCES Orders(Id)

);
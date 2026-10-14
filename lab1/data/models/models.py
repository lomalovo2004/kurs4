from sqlalchemy import Column, Integer, String, ForeignKey, Numeric, DateTime
from sqlalchemy.orm import declarative_base, relationship
from datetime import datetime

Base = declarative_base()

class Customer(Base):
    __tablename__ = "customers"
    id = Column(Integer, primary_key=True)
    name = Column(String(100), nullable=False)
    phone = Column(String(30), nullable=False)
    email = Column(String(100))

class Address(Base):
    __tablename__ = "addresses"
    id = Column(Integer, primary_key=True)
    customer_id = Column(Integer, ForeignKey("customers.id"), nullable=False)
    address_text = Column(String(255), nullable=False)

class Category(Base):
    __tablename__ = "categories"
    id = Column(Integer, primary_key=True)
    name = Column(String(100), nullable=False)

class Product(Base):
    __tablename__ = "products"
    id = Column(Integer, primary_key=True)
    category_id = Column(Integer, ForeignKey("categories.id"), nullable=False)
    name = Column(String(150), nullable=False)
    price = Column(Numeric(10, 2), nullable=False)

class OrderStatus(Base):
    __tablename__ = "order_statuses"
    id = Column(Integer, primary_key=True)
    name = Column(String(50), nullable=False)

class Order(Base):
    __tablename__ = "orders"
    id = Column(Integer, primary_key=True)
    customer_id = Column(Integer, ForeignKey("customers.id"), nullable=False)
    status_id = Column(Integer, ForeignKey("order_statuses.id"), nullable=False)
    created_at = Column(DateTime, default=datetime.utcnow)

class OrderItem(Base):
    __tablename__ = "order_items"
    id = Column(Integer, primary_key=True)
    order_id = Column(Integer, ForeignKey("orders.id"), nullable=False)
    product_id = Column(Integer, ForeignKey("products.id"), nullable=False)
    quantity = Column(Integer, nullable=False)

class Courier(Base):
    __tablename__ = "couriers"
    id = Column(Integer, primary_key=True)
    name = Column(String(100), nullable=False)
    phone = Column(String(30), nullable=False)

class Delivery(Base):
    __tablename__ = "deliveries"
    id = Column(Integer, primary_key=True)
    order_id = Column(Integer, ForeignKey("orders.id"), nullable=False)
    courier_id = Column(Integer, ForeignKey("couriers.id"))
    address_id = Column(Integer, ForeignKey("addresses.id"), nullable=False)
    delivery_status = Column(String(50), nullable=False)

class Payment(Base):
    __tablename__ = "payments"
    id = Column(Integer, primary_key=True)
    order_id = Column(Integer, ForeignKey("orders.id"), nullable=False)
    amount = Column(Numeric(10, 2), nullable=False)
    payment_method = Column(String(50), nullable=False)
    status = Column(String(50), nullable=False)

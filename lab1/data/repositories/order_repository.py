from data.models.models import Order

class OrderRepository:
    def __init__(self, db):
        self.db = db

    def get_by_id(self, order_id: int):
        return self.db.query(Order).filter(Order.id == order_id).first()

    def add(self, order: Order):
        self.db.add(order)
        self.db.commit()
        self.db.refresh(order)
        return order

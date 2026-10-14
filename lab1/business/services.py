from data.repositories.order_repository import OrderRepository

class OrderService:
    def __init__(self, db):
        self.repository = OrderRepository(db)

    def get_order(self, order_id: int):
        return self.repository.get_by_id(order_id)

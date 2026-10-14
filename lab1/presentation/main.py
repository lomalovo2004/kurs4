from fastapi import FastAPI, Depends
from sqlalchemy.orm import Session
from data.database import get_db
from business.services import OrderService

app = FastAPI(title="Delivery System")

@app.get("/")
def root():
    return {"message": "Delivery System API"}

@app.get("/orders/{order_id}")
def get_order(order_id: int, db: Session = Depends(get_db)):
    order = OrderService(db).get_order(order_id)
    if not order:
        return {"message": "Заказ не найден"}
    return {"id": order.id, "customer_id": order.customer_id, "status_id": order.status_id}

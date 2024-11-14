from datetime import datetime

from sqlalchemy import String, DateTime
from sqlalchemy.orm import DeclarativeBase, Mapped, mapped_column


class Base(DeclarativeBase):
    pass


class Member(Base):
    __tablename__ = "members"

    id: Mapped[int] = mapped_column(primary_key=True)
    discord_id: Mapped[str] = mapped_column(String, unique=True)
    username: Mapped[str] = mapped_column(String)
    rank: Mapped[str] = mapped_column(String)
    join_date: Mapped[datetime] = mapped_column(DateTime)
    last_active: Mapped[datetime] = mapped_column(DateTime)

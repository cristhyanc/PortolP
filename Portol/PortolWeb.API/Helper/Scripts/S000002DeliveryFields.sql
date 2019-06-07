GO
ALTER TABLE dbo.tblDriver ADD
	TotalTrips bigint NOT NULL CONSTRAINT DF_tblDriver_TotalTrips DEFAULT 0
GO
ALTER TABLE dbo.tblDriver ADD CONSTRAINT
	DF_tblDriver_Rating DEFAULT 5 FOR Rating
GO
ALTER TABLE dbo.tblDriver SET (LOCK_ESCALATION = TABLE)
GO
ALTER TABLE dbo.tblDelivery ADD
	Rating int NOT NULL CONSTRAINT DF_tblDelivery_Rating DEFAULT 0
GO
ALTER TABLE dbo.tblDelivery SET (LOCK_ESCALATION = TABLE)
GO



class Transaction():
	def __init__(timestamp_utc, transaction_type, description, amount):
		self.timestamp_utc = timestamp_utc
		self.transaction_type = transaction_type
		self.description = description
		self.amount = amount

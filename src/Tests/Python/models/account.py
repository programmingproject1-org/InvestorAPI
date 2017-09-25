class Account():
	def __init__(id, name, balance, positions = None):
		self.id = id
		self.name = name
		self.balance = balance
		self.positions = positions
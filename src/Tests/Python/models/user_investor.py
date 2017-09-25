#!/usr/bin/env python
# -*- coding: utf-8 -*-

class User:
	def __init__(self, displayName, email, password, level="Investor", accounts = None):
		self.displayName = displayName
		self.email = email
		self.password = password
		self.level = level
		self.accounts = accounts

	def __str__(self):
		return "[displayName: {0}], [email: {1}], [password: {2}], [level: {3}]".format(self.displayName, self.email, self.password, self.level)
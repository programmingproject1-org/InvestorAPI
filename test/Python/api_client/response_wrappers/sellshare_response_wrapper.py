#!/usr/bin/env python
# -*- coding: utf-8 -*-

import json
from pprint import pprint

class SellShareResponseWrapper():

	SUCCESS_STATUS = 200

	def __init__(self, response):
		self.response = response

	def get_http_status(self):
		return self.response.status_code

	def get_json_body(self):
		try:
			body = self.response.json()
		except ValueError:
			body = None
		return body

	def get_positions(self):
		positions = get_json_body()["positions"]
		return positions

	def get_balance(self):
		balance = get_json_body()["balance"]
		return balance

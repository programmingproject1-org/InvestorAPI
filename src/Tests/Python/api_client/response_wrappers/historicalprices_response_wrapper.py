#!/usr/bin/env python
# -*- coding: utf-8 -*-

import json

class HistoricalPricesResponseWrapper():

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

	def get_all_data_points(self):
		if self.get_json_body() is None:
			return None
		return self.get_json_body()["prices"]

	def get_message(self):
		try:
			body = self.get_json_body()
			message = body["message"]
		except:
			message = None
		return message
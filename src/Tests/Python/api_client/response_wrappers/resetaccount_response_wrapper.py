#!/usr/bin/env python
# -*- coding: utf-8 -*-

import json

class ResetAccountResponseWrapper:

	SUCCESS_STATUS = 201

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

	def get_message(self):
		try:
			body = self.get_json_body()
			message = body["message"]
		except:
			message = None
		return message

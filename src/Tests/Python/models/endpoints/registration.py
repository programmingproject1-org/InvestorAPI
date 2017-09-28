#!/usr/bin/env python
# -*- coding: utf-8 -*-

import requests
import json
from . import request_config

class Registration():
	def __init__(self, url, displayName, email, password):
		self.url = url
		self.displayName = displayName
		self.email = email
		self.password = password
		self.header = {"Content-Type": "application/json", "charset": "UTF-8"}
		self.init_payload()
		self.fetch()

	def init_payload(self):
		self.payload = {"displayName": self.displayName, "email": self.email, "password": self.password}

	def fetch(self):
		self.response = requests.post(self.url, data = json.dumps(self.payload, ensure_ascii=False)
			.encode('utf8'), headers = self.header, verify = request_config.VERIFY_HTTPS_REQUEST)

	def get_outcome(self):
		message = None

		if self.response.text:
			try:
				response_body = self.response.json()
				message = response_body["message"]
			except ValueError:
				pass

		return (self.response.status_code, message)
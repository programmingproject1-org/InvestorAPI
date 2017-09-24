#!/usr/bin/env python
# -*- coding: utf-8 -*-

import requests
import json

from models.response_summary import ResponseSummary

class Registration():
	def __init__(self, url, user):
		self.url = url
		self.user = user
		self.header = {"Content-Type": "application/json", "charset": "UTF-8"}
		self.init_payload()
		self.fetch()

	def init_payload(self):
		self.payload = {"displayName": self.user.displayName, "email": self.user.email, "password": self.user.password}

	def fetch(self):
		self.response = requests.post(self.url, data = json.dumps(self.payload, ensure_ascii=False).encode('utf8'), headers = self.header, verify = False)

	def get_outcome(self):
		error_messages = []

		if self.response.status_code == 201:
			is_success = True
		else:
			is_success = False
			response_body = self.response.json()

			if "message" in response_body:
				error_messages.append({"Message": response_body["message"]})

			if "validationErrors" in response_body:
				for validationError in response_body["validationErrors"]:
					error_messages.append({"Message": validationError})

			error_messages.append({"Failure on Test data": str(self.user)})

		self.response_summary = ResponseSummary(is_success, error_messages, self.response.status_code)

		return self.response_summary
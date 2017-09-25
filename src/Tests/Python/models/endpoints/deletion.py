#!/usr/bin/env python
# -*- coding: utf-8 -*-

import requests
import json

from models.response_summary import ResponseSummary

class Deletion():
	def __init__(self, url, user, token):
		self.url = url
		self.user = user
		if token is None:
			token = ""
		self.token = token
		self.header = {"Content-Type": "application/json", "Authorization": "Bearer " + token}
		self.fetch()

	def fetch(self):
		self.response = requests.delete(self.url, headers = self.header, verify = False)

	def get_outcome(self):
		error_messages = []

		if self.response.status_code == 204:
			is_success = True
		else:
			is_success = False
			
			try:
				response_body = self.response.json()
			except:
				response_body = {}

			if self.response.status_code == 401:
				error_messages.append({"Message": "Unauthorized"})

		self.response_summary = ResponseSummary(is_success, error_messages, self.response.status_code)

		return self.response_summary
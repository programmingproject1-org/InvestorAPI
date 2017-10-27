#!/usr/bin/env python
# -*- coding: utf-8 -*-

from requests import Request
import json
import time

from api_client.response_wrappers.resetaccount_response_wrapper import ResetAccountResponseWrapper

class ResetAccountRequest(Request):

	URL = "https://investor-api.herokuapp.com/api/1.0/accounts/ACCOUNT_ID"

	def __init__(self, session, token, account_id):
		self.token = token
		self.account_id = account_id
		self.make_request()
		self.session = session

	def make_request(self):
		self.URL = self.URL.replace('ACCOUNT_ID', str(self.account_id))
		headers = {
			"Content-Type": "application/json",
			"Authorization": "Bearer " + str(self.token)
		}
		super().__init__('PUT', url = self.URL, headers = headers)

	def get_response(self):
		response = self.session.send(super().prepare())
		wrapped_response = ResetAccountResponseWrapper(response)
		response.close()
		return wrapped_response
#!/usr/bin/env python
# -*- coding: utf-8 -*-

from requests import Request
import json
import time
from pprint import pprint

from api_client.response_wrappers.viewportfolio_response_wrapper import ViewPortfolioResponseWrapper

class ViewPortfolioRequest(Request):

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
		super().__init__('GET', url = self.URL, headers = headers)

	def get_response(self):
		response = self.session.send(super().prepare())
		wrapped_response = ViewPortfolioResponseWrapper(response)
		response.close()
		return wrapped_response